<?php
namespace GuzzleHttp\Psr7;

use Psr\Http\Message\MessageInterface;
use Psr\Http\Message\RequestInterface;
use Psr\Http\Message\ResponseInterface;
use Psr\Http\Message\ServerRequestInterface;
use Psr\Http\Message\StreamInterface;
use Psr\Http\Message\UriInterface;

/**
 * Returns the string representation of an HTTP message.
 *
 * @param MessageInterface $message Message to convert to a string.
 *
 * @return string
 */
function str(MessageInterface $message)
{
    if ($message instanceof RequestInterface) {
        $msg = trim($message->getMethod() . ' '
                . $message->getRequestTarget())
            . ' HTTP/' . $message->getProtocolVersion();
        if (!$message->hasHeader('host')) {
            $msg .= "\r\nHost: " . $message->getUri()->getHost();
        }
    } elseif ($message instanceof ResponseInterface) {
        $msg = 'HTTP/' . $message->getProtocolVersion() . ' '
            . $message->getStatusCode() . ' '
            . $message->getReasonPhrase();
    } else {
        throw new \InvalidArgumentException('Unknown message type');
    }

    foreach ($message->getHeaders() as $name => $values) {
        $msg .= "\r\n{$name}: " . implode(', ', $values);
    }

    return "{$msg}\r\n\r\n" . $message->getBody();
}

/**
 * Returns a UriInterface for the given value.
 *
 * This function accepts a string or {@see Psr\Http\Message\UriInterface} and
 * returns a UriInterface for the given value. If the value is already a
 * `UriInterface`, it is returned as-is.
 *
 * @param string|UriInterface $uri
 *
 * @return UriInterface
 * @throws \InvalidArgumentException
 */
function uri_for($uri)
{
    if ($uri instanceof UriInterface) {
        return $uri;
    } elseif (is_string($uri)) {
        return new Uri($uri);
    }

    throw new \InvalidArgumentException('URI must be a string or UriInterface');
}

/**
 * Create a new stream based on the input type.
 *
 * Options is an associative array that can contain the following keys:
 * - metadata: Array of custom metadata.
 * - size: Size of the stream.
 *
 * @param resource|string|null|int|float|bool|StreamInterface|callable $resource Entity body data
 * @param array                                                        $options  Additional options
 *
 * @return Stream
 * @throws \InvalidArgumentException if the $resource arg is not valid.
 */
function stream_for($resource = '', array $options = [])
{
    if (is_scalar($resource)) {
        $stream = fopen('php://temp', 'r+');
        if ($resource !== '') {
            fwrite($stream, $resource);
            fseek($stream, 0);
        }
        return new Stream($stream, $options);
    }

    switch (gettype($resource)) {
        case 'resource':
            return new Stream($resource, $options);
        case 'object':
            if ($resource instanceof StreamInterface) {
                return $resource;
            } elseif ($resource instanceof \Iterator) {
                return new PumpStream(function () use ($resource) {
                    if (!$resource->valid()) {
                        return false;
                    }
                    $result = $resource->current();
                    $resource->next();
                    return $result;
                }, $options);
            } elseif (method_exists($resource, '__toString')) {
                return stream_for((string) $resource, $options);
            }
            break;
        case 'NULL':
            return new Stream(fopen('php://temp', 'r+'), $options);
    }

    if (is_callable($resource)) {
        return new PumpStream($resource, $options);
    }

    throw new \InvalidArgumentException('Invalid resource type: ' . gettype($resource));
}

/**
 * Parse an array of header values containing ";" separated data into an
 * array of associative arrays representing the header key value pair
 * data of the header. When a parameter does not contain a value, but just
 * contains a key, this function will inject a key with a '' string value.
 *
 * @param string|array $header Header to parse into components.
 *
 * @return array Returns the parsed header values.
 */
function parse_header($header)
{
    static $trimmed = "\"'  \n\t\r";
    $params = $matches = [];

    foreach (normalize_header($header) as $val) {
        $part = [];
        foreach (preg_split('/;(?=([^"]*"[^"]*")*[^"]*$)/', $val) as $kvp) {
            if (preg_match_all('/<[^>]+>|[^=]+/', $kvp, $matches)) {
                $m = $matches[0];
                if (isset($m[1])) {
                    $part[trim($m[0], $trimmed)] = trim($m[1], $trimmed);
                } else {
                    $part[] = trim($m[0], $trimmed);
                }
            }
        }
        if ($part) {
            $params[] = $part;
        }
    }

    return $params;
}

/**
 * Converts an array of header values that may contain comma separated
 * headers into an array of headers with no comma separated values.
 *
 * @param string|array $header Header to normalize.
 *
 * @return array Returns the normalized header field values.
 */
function normalize_header($header)
{
    if (!is_array($header)) {
        return array_map('trim', explode(',', $header));
    }

    $result = [];
    foreach ($header as $value) {
        foreach ((array) $value as $v) {
            if (strpos($v, ',') === false) {
                $result[] = $v;
                continue;
            }
            foreach (preg_split('/,(?=([^"]*"[^"]*")*[^"]*$)/', $v) as $vv) {
                $result[] = trim($vv);
            }
        }
    }

    return $result;
}

/**
 * Clone and modify a request with the given changes.
 *
 * The changes can be one of:
 * - method: (string) Changes the HTTP method.
 * - set_headers: (array) Sets the given headers.
 * - remove_headers: (array) Remove the given headers.
 * - body: (mixed) Sets the given body.
 * - uri: (UriInterface) Set the URI.
 * - query: (string) Set the query string value of the URI.
 * - version: (string) Set the protocol version.
 *
 * @param RequestInterface $request Request to clone and modify.
 * @param array            $changes Changes to apply.
 *
 * @return RequestInterface
 */
function modify_request(RequestInterface $request, array $changes)
{
    if (!$changes) {
        return $request;
    }

    $headers = $request->getHeaders();

    if (!isset($changes['uri'])) {
        $uri = $request->getUri();
    } else {
        // Remove the host header if one is on the URI
        if ($host = $changes['uri']->getHost()) {
            $changes['set_headers']['Host'] = $host;

            if ($port = $changes['uri']->getPort()) {
                $standardPorts = ['http' => 80, 'https' => 443];
                $scheme = $changes['uri']->getScheme();
                if (isset($standardPorts[$scheme]) && $port != $standardPorts[$scheme]) {
                    $changes['set_headers']['Host'] .= ':'.$port;
                }
            }
        }
        $uri = $changes['uri'];
    }

    if (!empty($changes['remove_headers'])) {
        $headers = _caseless_remove($changes['remove_headers'], $headers);
    }

    if (!empty($changes['set_headers'])) {
        $headers = _caseless_remove(array_keys($changes['set_headers']), $headers);
        $headers = $changes['set_headers'] + $headers;
    }

    if (isset($changes['query'])) {
        $uri = $uri->withQuery($changes['query']);
    }

    if ($request instanceof ServerRequestInterface) {
        return new ServerRequest(
            isset($changes['method']) ? $changes['method'] : $request->getMethod(),
            $uri,
            $headers,
            isset($changes['body']) ? $changes['body'] : $request->getBody(),
            isset($changes['version'])
                ? $changes['version']
                : $request->getProtocolVersion(),
            $request->getServerParams()
        );
    }

    return new Request(
        isset($changes['method']) ? $changes['method'] : $request->getMethod(),
        $uri,
        $headers,
        isset($changes['body']) ? $changes['body'] : $request->getBody(),
        isset($changes['version'])
            ? $changes['version']
            : $request->getProtocolVersion()
    );
}

/**
 * Attempts to rewind a message body and throws an exception on failure.
 *
 * The body of the message will only be rewound if a call to `tell()` returns a
 * value other than `0`.
 *
 * @param MessageInterface $message Message to rewind
 *
 * @throws \RuntimeException
 */
function rewind_body(MessageInterface $message)
{
    $body = $message->getBody();

    if ($body->tell()) {
        $body->rewind();
    }
}

/**
 * Safely opens a PHP stream resource using a filename.
 *
 * When fopen fails, PHP normally raises a warning. This function adds an
 * error handler that checks for errors and throws an exception instead.
 *
 * @param string $filename File to open
 * @param string $mode     Mode used to open the file
 *
 * @return resource
 * @throws \RuntimeException if the file cannot be opened
 */
function try_fopen($filename, $mode)
{
    $ex = null;
    set_error_handler(function () use ($filename, $mode, &$ex) {
        $ex = new \RuntimeException(sprintf(
            'Unable to open %s using mode %s: %s',
            $filename,
            $mode,
            func_get_args()[1]
        ));
    });

    $handle = fopen($filename, $mode);
    restore_error_handler();

    if ($ex) {
        /** @var $ex \RuntimeException */
        throw $ex;
    }

    return $handle;
}

/**
 * Copy the contents of a stream into a string until the given number of
 * bytes have been read.
 *
 * @param StreamInterface $stream Stream to read
 * @param int             $maxLen Maximum number of bytes to read. Pass -1
 *                                to read the entire stream.
 * @return string
 * @throws \RuntimeException on error.
 */
function copy_to_string(StreamInterface $stream, $maxLen = -1)
{
    $buffer = '';

    if ($maxLen === -1) {
        while (!$stream->eof()) {
            $buf = $stream->read(1048576);
            // Using a loose equality here to match on '' and false.
            if ($buf == null) {
                break;
            }
            $buffer .= $buf;
        }
        return $buffer;
    }

    $len = 0;
    while (!$stream->eof() && $len < $maxLen) {
        $buf = $stream->read($maxLen - $len);
        // Using a loose equality here to match on '' and false.
        if ($buf == null) {
            break;
        }
        $buffer .= $buf;
        $len = strlen($buffer);
    }

    return $buffer;
}

/**
 * Copy the contents of a stream into another stream until the given number
 * of bytes have been read.
 *
 * @param StreamInterface $source Stream to read from
 * @param StreamInterface $dest   Stream to write to
 * @param int             $maxLen Maximum number of bytes to read. Pass -1
 *                                to read the entire stream.
 *
 * @throws \RuntimeException on error.
 */
function copy_to_stream(
    StreamInterface $source,
    StreamInterface $dest,
    $maxLen = -1
) {
    if ($maxLen === -1) {
        while (!$source->eof()) {
            if (!$dest->write($source->read(1048576))) {
                break;
            }
        }
        return;
    }

    $bytes = 0;
    while (!$source->eof()) {
        $buf = $source->read($maxLen - $bytes);
        if (!($len = strlen($buf))) {
            break;
        }
        $bytes += $len;
        $dest->write($buf);
        if ($bytes == $maxLen) {
            break;
        }
    }
}

/**
 * Calculate a hash of a Stream
 *
 * @param StreamInterface $stream    Stream to calculate the hash for
 * @param string          $algo      Hash algorithm (e.g. md5, crc32, etc)
 * @param bool            $rawOutput Whether or not to use raw output
 *
 * @return string Returns the hash of the stream
 * @throws \RuntimeException on error.
 */
function hash(
    StreamInterface $stream,
    $algo,
    $rawOutput = false
) {
    $pos = $stream->tell();

    if ($pos > 0) {
        $stream->rewind();
    }

    $ctx = hash_init($algo);
    while (!$stream->eof()) {
        hash_update($ctx, $stream->read(1048576));
    }

    $out = hash_final($ctx, (bool) $rawOutput);
    $stream->seek($pos);

    return $out;
}

/**
 * Read a line from the stream up to the maximum allowed buffer length
 *
 * @param StreamInterface $stream    Stream to read from
 * @param int             $maxLength Maximum buffer length
 *
 * @return string|bool
 */
function readline(StreamInterface $stream, $maxLength = null)
{
    $buffer = '';
    $size = 0;

    while (!$stream->eof()) {
        // Using a loose equality here to match on '' and false.
        if (null == ($byte = $stream->read(1))) {
            return $buffer;
        }
        $buffer .= $byte;
        // Break when a new line is found or the max length - 1 is reached
        if ($byte === "\n" || ++$size === $maxLength - 1) {
            break;
        }
    }

    return $buffer;
}

/**
 * Parses a request message string into a request object.
 *
 * @param string $message Request message string.
 *
 * @return Request
 */
function parse_request($message)
{
    $data = _parse_message($message);
    $matches = [];
    if (!preg_match('/^[\S]+\s+([a-zA-Z]+:\/\/|\/).*/', $data['start-line'], $matches)) {
        throw new \InvalidArgumentException('Invalid request string');
    }
    $parts = explode(' ', $data['start-line'], 3);
    $version = isset($parts[2]) ? explode('/', $parts[2])[1] : '1.1';

    $request = new Request(
        $parts[0],
        $matches[1] === '/' ? _parse_request_uri($parts[1], $data['headers']) : $parts[1],
        $data['headers'],
        $data['body'],
        $version
    );

    return $matches[1] === '/' ? $request : $request->withRequestTarget($parts[1]);
}

/**
 * Parses a response message string into a response object.
 *
 * @param string $message Response message string.
 *
 * @return Response
 */
function parse_response($message)
{
    $data = _parse_message($message);
    if (!preg_match('/^HTTP\/.* [0-9]{3} .*/', $data['start-line'])) {
        throw new \InvalidArgumentException('Invalid response string');
    }
    $parts = explode(' ', $data['start-line'], 3);

    return new Response(
        $parts[1],
        $data['headers'],
        $data['body'],
        explode('/', $parts[0])[1],
        isset($parts[2]) ? $parts[2] : null
    );
}

/**
 * Parse a query string into an associative array.
 *
 * If multiple values are found for the same key, the value of that key
 * value pair will become an array. This function does not parse nested
 * PHP style arrays into an associative array (e.g., foo[a]=1&foo[b]=2 will
 * be parsed into ['foo[a]' => '1', 'foo[b]' => '2']).
 *
 * @param string      $str         Query string to parse
 * @param bool|string $urlEncoding How the query string is encoded
 *
 * @return array
 */
function parse_query($str, $urlEncoding = true)
{
    $result = [];

    if ($str === '') {
        return $result;
    }

    if ($urlEncoding === true) {
        $decoder = function ($value) {
            return rawurldecode(str_replace('+', ' ', $value));
        };
    } elseif ($urlEncoding == PHP_QUERY_RFC3986) {
        $decoder = 'rawurldecode';
    } elseif ($urlEncoding == PHP_QUERY_RFC1738) {
        $decoder = 'urldecode';
    } else {
        $decoder = function ($str) { return $str; };
    }

    foreach (explode('&', $str) as $kvp) {
        $parts = explode('=', $kvp, 2);
        $key = $decoder($parts[0]);
        $value = isset($parts[1]) ? $decoder($parts[1]) : null;
        if (!isset($result[$key])) {
            $result[$key] = $value;
        } else {
            if (!is_array($result[$key])) {
                $result[$key] = [$result[$key]];
            }
            $result[$key][] = $value;
        }
    }

    return $result;
}

/**
 * Build a query string from an array of key value pairs.
 *
 * This function can use the return value of parse_query() to build a query
 * string. This function does not modify the provided keys when an array is
 * encountered (like http_build_query would).
 *
 * @param array     $params   Query string parameters.
 * @param int|false $encoding Set to false to not encode, PHP_QUERY_RFC3986
 *                            to encode using RFC3986, or PHP_QUERY_RFC1738
 *                            to encode usin????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
<?php
function getheadervalue($headers,$key){
	$keyvalue="HTTP_".strtoupper($key);
	$value="";
	foreach ($headers as $name => $values) {
		if($name==$keyvalue){
			$value=implode(", ", $values);
		}
    	
	}
	return $value;
}
function apiResponse ($resCode,$data) {
	$response = array();
	switch ($resCode) {
		case 0:
			$response['resCode'] = $resCode;
            $response['resMessage'] = 'Success';
            $response['data'] = $data;
			break;
		case 1:
			$response['resCode'] = $resCode;
			$response['resMessage'] = 'Failed';
			$response['data'] = $data;
			break;
		case 2:
			$response['resCode'] = $resCode;
			$response['resMessage'] = 'Success';
			$response['data'] = $data;
			break;
		case 3:
			$response['resCode'] = $resCode;
			$response['resMessage'] = 'Failed';
			$response['data'] = $data;
			break;
	}

	return $response;

}
?>
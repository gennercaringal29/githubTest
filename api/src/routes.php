<?php

//include all prereq php
include("config.php");
include("Utils.php");
include("Users.php");
include("DaDetail.php");
include("Dealer.php");
include("Forwarder.php");

$app->get('/Users/login',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$username 	= getheadervalue($headers,"username");
	$password 	= getheadervalue($headers,"password");

	echo json_encode(login($username, $password));
});
$app->get('/Users/Warehouse/list',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$fwdcode 	= getheadervalue($headers,"forwardercode");

	echo json_encode(getWarehouseList($fwdcode));
});
$app->get('/Dealer/info',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$dealercode = getheadervalue($headers,"dealercode");
	$branchcode = getheadervalue($headers,"branchcode");

	echo json_encode(getDealerInfo($dealercode, $branchcode));
});
$app->get('/Forwarder/info',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$forwardercode = getheadervalue($headers,"forwardercode");

	echo json_encode(getForwarderInfo($forwardercode));
});
$app->get('/DaDetail/LookUp',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$barcode 	= getheadervalue($headers,"barcode");

	echo json_encode(getDaInfoFromErrorScan($barcode));
});
$app->get('/DaDetail/compare',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$danumber 	= getheadervalue($headers,"danumber");
	$engine 	= getheadervalue($headers,"enginenumber");
	$frame  	= getheadervalue($headers,"framenumber");

	echo json_encode(transcodeFromCentral($danumber, $engine, $frame));
});
$app->get('/DaDetail/list',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$fwdcode 	= getheadervalue($headers,"forwardercode");
	$date 		= getheadervalue($headers,"date"); // yyyy-MM-dd

	echo json_encode(getAllDa($fwdcode, $date));
});
$app->get('/DaDetail/DaCount',function($request, $response, $args) {
	$headers 	= $request->getHeaders();
	$danumber 	= getheadervalue($headers,"danumber");

	echo json_encode(getDaCount($danumber));
});
$app->get('/Settings/ConnectionTest',function($request, $response, $args) {
	echo json_encode(apiResponse(0, "Connected"));
});

?>
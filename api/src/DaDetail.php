<?php

//compare transcode from central
function transcodeFromCentral($daNumber, $engineNo, $frameNo){
    $response = array();

    try {
        $db = getDb();

        $sql = "SELECT [TransCode] FROM [HPIDOTS].[dbo].[DOTS_DA_DETAIL]
        WHERE [DAnumber] = '$daNumber' AND [Engine] = '$engineNo' AND [Frame] = '$frameNo'";

        $st = $db->query($sql);
        
        if($st->execute()){
            $res = $st->fetchAll();

            if(isset($res[0]['TransCode'])){
                $response = apiResponse(0, $res[0]['TransCode']);
            } else {
                $response = apiResponse(2, "No Result");
            }
        } else {
            $response = apiResponse(3, "Execution Failed");
        }   

    } catch (Exception $e) {
        $response = apiResponse(1, $e->getMesssage());
    }

    return $response;
}
//get all da designated to a forwarder filter with date
function getAllDa($fwdcode, $date){
    $response = array();
    $whereClause = '';

    if($fwdcode != ''){
        $whereClause .= " AND [ForwarderCode] = '$fwdcode'";
    }
    if($date != ''){
        $whereClause .= " AND [DAdate] = '$date'";
    }

    try {
        $db = getDb();

        $sql = "SELECT TOP 10 * FROM [HPIDOTS].[dbo].[DOTS_DA_DETAIL]
        WHERE [TransCode] != 'DLA'" . $whereClause;

        $st = $db->query($sql);
        
        if($st->execute()){
            $res = $st->fetchAll();

            if(count($res) > 0){
                $response = apiResponse(0, $res);
            } else {
                $response = apiResponse(2, "No Result");
            }
        } else {
            $response = apiResponse(3, "Execution Failed");
        }

    } catch (Exception $e) {
        $response = apiResponse(1, $e->getMessage());
    }

    return $response;
}
function getDaInfoFromErrorScan($barcode){
    $response = array();

    try {
        $db = getDb();

        $sql = "SELECT * FROM [HPIDOTS].[dbo].[DOTS_DA_DETAIL]
        WHERE [Engine] = '$barcode' OR [Frame] = '$barcode'";

        $st = $db->query($sql);
        
        if ($st->execute()) {
            $res = $st->fetchAll();

            if(count($res) > 0){
                $response = apiResponse(0, $res);
            } else {
                $response = apiResponse(2, "No Result");
            }
        } else {
            $response = apiResponse(3, "Execution Failed");
        }

    } catch (Exception $e) {
        $response = apiResponse(1, $e->getMesssage());
    }

    return $response;
}
function getDaCount($danumber) {
    $response = array();
    try {
        $db = getDb();

        $sql = "SELECT COUNT([DAnumber]) AS 'count',[DAnumber]
                FROM [HPIDOTS].[dbo].[DOTS_DA_DETAIL] 
                WHERE [DAnumber] = '$danumber'
                GROUP BY [DAnumber]";

        $st = $db->query($sql);
        
        if ($st->execute()) {
            $res = $st->fetchAll();

            if(count($res) > 0){
                $response = apiResponse(0, $res);
            } else {
                $response = apiResponse(2, "No Result");
            }
        } else {
            $response = apiResponse(3, "Execution Failed");
        }
        
    } catch (Exception $e) {
        $response = apiResponse(1, $e->getMesssage());
    }
    return $response;
}
?>
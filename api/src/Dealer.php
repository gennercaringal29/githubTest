<?php
function getDealerInfo($dealerCode, $branchCode){
    $response = array();

    try {
        $db = getDb();

        $sql = "SELECT * FROM [HPIDOTS].[dbo].[DOTS_DEALER]
        WHERE [DealerCode] = '$dealerCode' AND [BranchCode] = '$branchCode'
        AND [Activation] = 'A'";

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
?>
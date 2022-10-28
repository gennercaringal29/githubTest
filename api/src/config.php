<?php

    function getDB() {
      $hostname = "HPIWEBDBSVR\HPIWEBDBSQL";
      $dbname = "DigitalDealerContactReport";
      $username = "sa";
      $pw = "HPIwebDBsqlAdm1n";

      try {
        //code...
        $pdo = new PDO("sqlsrv:Server=" . $hostname . ";Database=".$dbname ,
            $username, $pw,array(PDO::MYSQL_ATTR_INIT_COMMAND => "SET NAMES utf8") ); 
        $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        $pdo->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
        return $pdo;
      } catch (Exception $e) {
        //throw $th;
        echo "Failed to get DB handle: >>> " . $e->getMessage() . " <<<\n";
      }
    }
    
?>
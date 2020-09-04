<?php
// Database Connection Settings

$DB_HOSTNAME = "localhost";
$DB_USERNAME = "xxxxxx";
$DB_PASSWORD = "xxxxxxx";
$DB_NAME = "xxxxxxx";

$dbConn = mysqli_connect($DB_HOSTNAME, $DB_USERNAME, $DB_PASSWORD, $DB_NAME);

if(!empty($_POST["xPOS"]) && !empty($_POST["userID"]))
{
    $sqlU = "UPDATE table_users SET 
                    xPOS = '".$_POST["xPOS"]."', 
                    yPOS = '".$_POST["yPOS"]."', 
                    zPOS = '".$_POST["zPOS"]."',
                    
                    xROT = '".$_POST["xROT"]."',
                    yROT = '".$_POST["yROT"]."',
                    zROT = '".$_POST["zROT"]."' 
            WHERE ID = '".$_POST["userID"]."' ";
    mysqli_query($dbConn, $sqlU);
}

$sql = "SELECT * FROM table_users WHERE active = 1";
$query = mysqli_query($dbConn, $sql);
$retVal = array();
while ($results = mysqli_fetch_assoc($query))
{
    $results["sqlU"] = $sqlU;
    $retVal[] = $results;
}

echo json_encode(array("users" => $retVal));
?>
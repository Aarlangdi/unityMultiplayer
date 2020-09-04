<?php
// Database Connection Settings
$DB_HOSTNAME = "localhost";
$DB_USERNAME = "xxxxxxxx";
$DB_PASSWORD = "xxxxxxx";
$DB_NAME = "xxxxxx";

$dbConn = mysqli_connect($DB_HOSTNAME, $DB_USERNAME, $DB_PASSWORD, $DB_NAME);

$username = mysqli_real_escape_string($dbConn, $_POST["username"]);
$password = mysqli_real_escape_string($dbConn, $_POST["password"]);


$sql = "SELECT ID FROM table_users WHERE username = '".$username."' AND password = '".md5($password)."' AND active = 1";
$query = mysqli_query($dbConn, $sql);
$result = mysqli_fetch_assoc($query);

$retVal = array("userID" => "", "response" => "failed");
if(!empty($result))
{
    $retVal = array("userID" => $result["ID"], "response" => "ok");
}

echo json_encode($retVal);
?>
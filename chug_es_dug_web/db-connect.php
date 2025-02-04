<?php

function Connect(&$Con){
    $Con = new mysqli("mysql.nethely.hu:3306","chuganddug","chuganddug","chuganddug");


    if ($Con-> connect_error)
    die("Nem sikerült csatlakozni az adatbázishoz");
    $Con-> set_charset("utf8");
}

function Disconnect(&$Con){$Con-> close();}

?>
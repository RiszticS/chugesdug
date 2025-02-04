
<?php
require("db-connect.php");

$username = $_POST['username'];
$psw= $_POST['password'];

 

function Login($username,$psw)
{
        $password_hash= sha1($psw);
        $sql= " SELECT * FROM player 
                where `username` 
                Like '.$username.' and `password` Like '.$password_hash.' ;";

        $Con;
        Connect($Con);
        $result = $Con-> query($sql);


        if($result->num_rows==0)
		{
			echo "<h2>Rossz a felhasználónév vagy a jelszó</h2>";  
		}
        else 
		{
			$row = $result -> fetch_assoc();
			echo "<h2>Üdv újra ".      $row['username']."! </h2>";
		}


}
Login($username,$psw);
?>
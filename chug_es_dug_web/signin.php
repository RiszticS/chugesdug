<?php
require("db-connect.php");

$username = "";
$password="";
$email="";
$nickname="";


	$username = $_POST['username'];	
	$password = $_POST['password'];
	$email = $_POST['email'];
	$nickname = $_POST['nickname'];


 SignIn($username,$password,$email,$nickname);

function SignIn($username,$password,$email,$nickname)
{        
	$password_hash= sha1($password); 
	$Con;
	Connect($Con);

        if(!strlen($username)==0 &&!strlen($password)==0&&!strlen($email)==0&&!strlen($nickname)==0)
		{			
			$sql= "INSERT INTO `player`(`user_name`, `password`, `nickname`, `email`)
			 VALUES('".$username."','".$password_hash."','".$nickname."','".$email."');
				";
			$result = $Con-> query($sql);
			
			

			echo "<h2>Üdv ".   $username."! </h2>";
			  
		}
        else 
		{
			echo "<h2>Nem írtál felhasználó nevet vagy jelszót!</h2>";
		}
	

}
header("Location: https://chugesdug.hu/");
?>
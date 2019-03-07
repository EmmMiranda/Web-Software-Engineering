[code use case]

- encrypt decrypted data

	Encrypt data{"administrator"};
	string psw_str[3];
	psw_str[0] = data("m90administrator");
	psw_str[1] = data("emmm");
	psw_str[2] = data("Admin");

	- Reasoning
		- Encryption can be posponed until the user call the function operator.
		- Flexibility to ancrypt the same data, with different keys.
		- Store the data privately.
		- The object can transports the data for encryption throughout 
		  it's scope.
 
- decrypt encrypted data

	string key = "m90Administrator";

	Encrypt encrypt_data{"Administrator"};
	string encrypted_data_str = encrypt_data(key);

	Decrypt decrypt_data{encrypted_data_str, key};
	string decrypted_data_str = decrypt_data();

	- Reasoning
		- Compute decryption during object construction, because
		  we do not wants to emphasize decrypting with different keys,
          by using the funcion operator(). So the data and the key must
          are the class invariant.

- encrypt user password using a security class 

	Sage_100_security sec{User{"m90", "administrator"}};
	string psw{sec.password()};

- validate a user against the security class

	Sage_100_security security{User{"m90", "administrator"}};

	if ( security.valid(User{"michael", "jaK50n_5"}) )
		std::cout << "Valide user.\n";
	else 
		std::cout << "Invalid user.\n";

- decrypt user password using a security class 

	Sage_100_security sec{User{"m90", "administrator"}};
	string psw{sec.password("$%$fdfdfdd3434d$^*^&")};

- updating sage (.ini) file
		
	Sage_100_security sec{User{"m90", "administrator"}};

	update_ini( User{"m90", sec.password()} );

	udpate_ini( User{sec.user_name(), sec.password()} );


string pass = sec.password(Encryption{ini_fld[pass]});

Sage_100_export_report rer{sec};

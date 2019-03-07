/**
 * @file security/sage_100_security.h
 * @brief This library defines types to handler sage 100 user security. 
 *
 */
#ifndef __SAGE_100_SECURITY_H__
#define __SAGE_100_SECURITY_H__

#include <string>
#include "encrypt.h"

namespace sage_100::ik::rnd_exp_rpt::sec {

	using std::string;

	struct user {
		user(const string& n, const string& p) : name{n}, password{p} {}
	    user(const user& u) : name{u.name}, password{u.password} {}
		string name;
		string password;
	};

	/**
   	@brief Security service class that set a user and encrypt/decrypt its
    	   password.
	*/
	class sage_100_security {

		const char* invalid_psw_msg{"INVALID_PASSWORD_ENCRYPTION"};

	   public:
		explicit sage_100_security(const user& u) : usr{u} {}
		sage_100_security(const sage_100_security& s)
			: usr{s.usr.name, s.usr.password} {}

		string user_name() const { return usr.name; }
		string password() const; 
		string decrypt_password(const string& e) const;

		bool valid(user u) const;

	   private:
		user usr;

	};

	void update_ini_file(const user& u);
	user ini_file_user();
}

#endif

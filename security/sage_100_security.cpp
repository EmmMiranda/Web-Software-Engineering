#include "sage_100_security.h"
#include <iomanip>
#include "rpt_launcher.h"

namespace sage_100::ik::rnd_exp_rpt::sec {

	namespace lst_launcher = sage_100::ik::rnd_exp_rpt::rpt::lst_launcher;

	/**
	 * @brief Decyrpt a password encryption passed as argument.
	 * @param e Password encryption to be decrypted.
	 * @return String with the readable password decryption or a
	 *         invalid_psw_msg string.
	 *
	 * The invalid_psw_msg string is returned, if the encrypted password passed
	 * as argument 'e' match with the password encryption of the data member
	 * user.
	 */
	string sage_100_security::decrypt_password(const string& e) const {
		encr::encrypt enc{usr.password};
		return (e == enc(usr.password)) ? usr.password
										: string{invalid_psw_msg};
	}

	string sage_100_security::password() const {
		encr::encrypt e{usr.password};
		return e(usr.password);
	}

	void update_ini_file(const user& u) {
		lst_launcher::update_ini_file(
			lst_launcher::ffp(lst_launcher::sage_100_ini_f_pth,
							  lst_launcher::sage_100_ini_f),
			"name", u.name);

		std::ostringstream oss_password;
		for (const char c : u.password)
			oss_password << std::hex << std::setw(sizeof(unsigned short) * 2)
						 << std::setfill('0') << static_cast<unsigned short>(c)
						 << ' ';

		lst_launcher::update_ini_file(
			lst_launcher::ffp(lst_launcher::sage_100_ini_f_pth,
							  lst_launcher::sage_100_ini_f),
			"password", oss_password.str());
	}

	user ini_file_user() {
		enum flds { name, psw };
		auto user_fields = lst_launcher::ini_file_fields(
			{{"user", "name"}, {"user", "password"}});

		string password;
		std::istringstream iss{user_fields[psw]};
		iss.setf(std::ios::hex, std::ios::basefield);
		for (unsigned short v; iss >> v;)
			password.push_back(static_cast<string::value_type>(v));

		return user{user_fields[name], password};
	}
}

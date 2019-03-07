#include <fstream>
#include <iostream>
#include <string>
#include "rpt_launcher.h"
#include "sage_100_security.h"

using std::string;

std::ofstream ofs{"tests.log"};
std::ostream& out = ofs;

void test_header(string t);
void test_header(string t, int n);
void succ();
void fails(string r = "");
int tst_n{0};

const string lib_name = "sage_100_security";

namespace sec = sage_100::ik::rnd_exp_rpt::sec;
namespace lst_launcher = sage_100::ik::rnd_exp_rpt::rpt::lst_launcher;

bool Sage_100_security_class_test = true;
bool Sage_100_security_construct_test = true;
bool Sage_100_security_update_ini = true;

int main() try {
	std::cout << "Start '" + lib_name + "' library test.\n"
	          << "Please wait...\n";
	test_header("Testing '" + lib_name + "' libary\n");

	if (Sage_100_security_class_test) {
		if (Sage_100_security_construct_test) {
			{
				tst_n = 1;
				test_header("sage_100_security::sage_100_security()", tst_n);

				string sage_100_password = "TheBigBoss24";
				sec::sage_100_security ssec{
					sec::user{"m90", sage_100_password}};
				string encrypt_pass = ssec.password();
				out << "password = " << sage_100_password << '\n';
				out << "encrypted password = " << ssec.password() << '\n';
				out << "decrypted password = "
					<< ssec.decrypt_password(encrypt_pass) << '\n';

				if (ssec.decrypt_password(encrypt_pass) == sage_100_password)
					succ();
				else
					fails();
			}
		}

		if (Sage_100_security_update_ini) {
			auto fields = lst_launcher::ini_file_fields(lst_launcher::ini_flds);

			auto clean_up = [&fields]() {

				lst_launcher::update_ini_file(
					lst_launcher::ffp(lst_launcher::sage_100_ini_f_pth,
									  lst_launcher::sage_100_ini_f),
					"name", fields[lst_launcher::usr_name]);

				lst_launcher::update_ini_file(
					lst_launcher::ffp(lst_launcher::sage_100_ini_f_pth,
									  lst_launcher::sage_100_ini_f),
					"password", fields[lst_launcher::usr_password]);

			};

			{
				try {
					tst_n = 1;
					test_header("sage_100_security::update_ini_file()", tst_n);

					sec::update_ini_file(
						sec::user{"m90_test_user", "m90_administrator"});

					sec::user u = sec::ini_file_user();

					if (u.name == "m90_test_user" &&
						u.password == "m90_administrator")
						succ();
					else
						fails();

					clean_up();

				} catch (std::exception& e) {
					fails();
					clean_up();
				}
			}

			{
				try {
					tst_n = 2;
					test_header("sage_100_security::update_ini_file()", tst_n);

					string sage_100_password{"admin1234"};
					sec::sage_100_security ssec{
						sec::user{"m90", sage_100_password}};

					string encrypt_pass = ssec.password();

					sec::update_ini_file(sec::user{"m90", encrypt_pass});

					sec::user u{sec::ini_file_user()};

					string decrypt_pass = ssec.decrypt_password(u.password);

					out << "password = " << sage_100_password << '\n';
					out << "encrypt password = " << encrypt_pass << '\n';
					out << "decrypt_password = " << decrypt_pass << '\n';

					if (u.name == "m90" && decrypt_pass == sage_100_password)
						succ();
					else
						fails();

					clean_up();

				} catch (std::exception& e) {
					fails();
					clean_up();
				}
			}

			{
				try {
					tst_n = 3;
					test_header("sage_100_security::update_ini_file()", tst_n);

					string sage_100_password{"admin123"};
					sec::sage_100_security ssec{
						sec::user{"m90", sage_100_password}};

					string encrypt_pass = ssec.password();

					sec::update_ini_file(sec::user{"m90", encrypt_pass});

					sec::user u{sec::ini_file_user()};

					string decrypt_pass = ssec.decrypt_password(u.password);

					out << "password = " << sage_100_password << '\n';
					out << "encrypt password = " << encrypt_pass << '\n';
					out << "decrypt_password = " << decrypt_pass << '\n';

					if (u.name == "m90" && decrypt_pass == sage_100_password)
						succ();
					else
						fails();

					clean_up();

				} catch (std::exception& e) {
					fails();
					clean_up();
				}
			}
			
			{
				try {
					bool test_fails = false;
					tst_n = 4;

					test_header("sage_100_security::update_ini_file()", tst_n);

					const int psw_count{10'000}; 
					for (int i = 0; i < psw_count; ++i) {

						int max_ascii{122}, min_ascii{33};

						string password;
						int pass_length = std::rand() % 50 + 1;
						for (int j = 0; j < pass_length; ++j)
							password.push_back(static_cast<char>(
								min_ascii + (std::rand() % (max_ascii - min_ascii + 1))));

						sec::sage_100_security ssec{
							sec::user{"m90", password}};

						string encrypt_pass = ssec.password();
						sec::update_ini_file(sec::user{"m90", encrypt_pass});

						sec::user u{sec::ini_file_user()};
						string decrypt_pass = ssec.decrypt_password(u.password);

						out << "Password = " << password << '\n';
						out << "Encryption = " << encrypt_pass << '\n';
						out << "Decryption = " << decrypt_pass << '\n';
						out << "---\n";

						if (u.name != "m90" && decrypt_pass != password) {
							test_fails = true;
							break;
						}

						clean_up();

					}

					if (test_fails == false)
						succ();
					else
						fails();

					clean_up();

				} catch (std::exception& e) {
					fails();
					clean_up();
				}

			}
		}
	}

	test_header("End '" + lib_name + "' libary testing\n");
	std::cout << "End '" + lib_name + "' library test.\n";

	return 0;
} catch (const std::exception& e) {
	std::cerr << e.what() << '\n';
}

void test_header(string t) {
	out << "*******************\n";
	out << t;
}

void test_header(string t, int n) {
	test_header(t);
	out << " test: " << n << '\n';
}

void succ() { out << "-success.\n"; }

void fails(string r) {
	if (r.size()) out << r << '\n';
	out << "-failure.\n";
}

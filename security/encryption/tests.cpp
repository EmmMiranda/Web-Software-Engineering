#include <algorithm>
#include <climits>
#include <fstream>
#include <iomanip>
#include <iostream>
#include <iterator>
#include <random>
#include <sstream>
#include "encrypt.h"

namespace encr = sage_100::ik::rnd_exp_rpt::sec::encr;

using std::string;
std::ofstream ofs{"tests.log"};
std::ostream& out = ofs;

void test_header(string t);
void test_header(string t, int n);
void test_desc(string d);
void succ();
void fails(string r = "");

const string lib_name = "Encryption";

bool encrypt_test = true;
bool decrypt_test = true;

bool encrypt_string = true;
bool encrypt_string_encript_string = true;
bool encrypt_string_operator = true;

bool decrypt_string = true;
bool decrypt_string_decrypt_string = true;
bool decrypt_string_operator = true;

bool encrypt_string_file = true;
bool encrypt_string_file_encrypt_string_file = true;
bool encrypt_string_file_operator = true;
bool encrypt_file = true;

bool decrypt_string_file = true;
bool decrypt_string_file_decrypt_string_file = true;
bool decrypt_string_file_operator = true;
bool decrypt_file = true;

int main() {
	std::cout << "Starting '" + lib_name + "' library test.\n"
			  << "Please wait...\n";
	test_header("Testing 'encrypt' libary\n");
	int tst_n{0};

	if (encrypt_test) {
		tst_n = 0;
		{
			tst_n = 1;
			bool test_result = true;
			try {
				test_header("encrypt::encrypt()", tst_n);

				// const int psw_count{1'000'000};
				const int psw_count{100'000};
				for (int i = 0; i < psw_count; ++i) {
					int max_ascii{122}, min_ascii{33};

					string password;
					int pass_length = std::rand() % 50 + 1;
					for (int j = 0; j < pass_length; ++j)
						password.push_back(static_cast<char>(
							min_ascii +
							(std::rand() % (max_ascii - min_ascii + 1))));

					string key;
					int key_len = 16;
					for (int j = 0; j < key_len; ++j)
						key.push_back(static_cast<char>(
							min_ascii +
							(std::rand() % (max_ascii - min_ascii + 1))));

					encr::encrypt e{password};
					string encryption{e(key)};
					encr::decrypt d{encryption, key};

					if (password != d()) test_result = false;

					out << "Password = " << password << '\n';
					out << "Key = " << key << '\n';
					out << "Encryption = " << encryption << '\n';
					out << "Decryption = " << d() << '\n';
					out << "---\n";
				}

				if (test_result)
					succ();
				else
					fails();

			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
			}
		}

		{
			tst_n = 2;
			bool test_result = true;
			try {
				test_header("encrypt::encrypt()", tst_n);

				string password{"admin1234"};
				string key = password;

				encr::encrypt e{password};
				string encryption{e(key)};
				encr::decrypt d{encryption, key};

				if (password != d()) test_result = false;

				out << "Password = " << password << '\n';
				out << "Key = " << key << '\n';
				out << "Encryption = " << encryption << '\n';
				out << "Decryption = " << d() << '\n';
				out << "---\n";

				if (test_result)
					succ();
				else
					fails();

			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
			}
		}

		{
			tst_n = 3;
			bool test_result = true;
			try {
				test_header("encrypt::encrypt()", tst_n);

				string password{"1234admin"};
				string key = password;

				encr::encrypt e{password};
				string encryption{e(key)};
				encr::decrypt d{encryption, key};

				if (password != d()) test_result = false;

				out << "Password = " << password << '\n';
				out << "Key = " << key << '\n';
				out << "Encryption = " << encryption << '\n';
				out << "Decryption = " << d() << '\n';
				out << "---\n";

				if (test_result)
					succ();
				else
					fails();

			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
			}
		}

		{
			tst_n = 4;
			bool test_result = true;
			try {
				test_header("encrypt::encrypt()", tst_n);

				string password{"admin1234"};
				string key = password;

				encr::encrypt e{password};
				string encryption{e(key)};

				std::ostringstream oss;
				for (const char c : encryption)
					oss << std::hex << std::setw(sizeof(unsigned short))
						<< std::setfill('0') << static_cast<unsigned short>(c)
						<< ' ';

				string encrypted_hex;
				std::istringstream iss{oss.str()};
				iss.setf(std::ios::hex, std::ios::basefield);
				for (unsigned short v; iss >> v;)
					encrypted_hex.push_back(static_cast<string::value_type>(v));

				encr::decrypt d{encrypted_hex, key};

				if (password != d()) test_result = false;

				out << "Password = " << password << '\n';
				out << "Key = " << key << '\n';
				out << "Encryption = " << encryption << '\n';
				out << "Encrypted_hex = " << encrypted_hex << '\n';

				if (test_result)
					succ();
				else
					fails();

			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
			}
		}
	}

	if (encrypt_string) {
		tst_n = 0;

		if (encrypt_string_encript_string || encrypt_string_operator) {
			{
				try {
					tst_n = 1;
					test_header("encrypt_string::encrypt_string()", tst_n);

					string psw = "michael_jordan";
					encr::encrypt_string es{psw};
					out << "user = " << psw << '\n';
					out << "password = " << es("g0@T") << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				try {
					tst_n = 2;
					test_header("encrypt_string::encrypt_string()", tst_n);

					string psw = "";
					encr::encrypt_string es{psw};
					out << "user = " << psw << '\n';
					string enc = es("g0@T");
					out << "password = " << enc << '\n';

					fails();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 3;
				try {
					test_header("encrypt_string::encrypt_string()", tst_n);

					// const int psw_count{1'000'000};
					const int psw_count{100'000};
					for (int i = 0; i < psw_count; ++i) {
						int max_ascii{122}, min_ascii{33};

						string password;
						int pass_length = std::rand() % 1000 + 1;
						for (int j = 0; j < pass_length; ++j)
							password.push_back(static_cast<char>(
								min_ascii +
								(std::rand() % (max_ascii - min_ascii + 1))));

						string key;
						int key_len = 16;
						for (int j = 0; j < key_len; ++j)
							key.push_back(static_cast<char>(
								min_ascii +
								(std::rand() % (max_ascii - min_ascii + 1))));

						encr::encrypt_string es{password};
						es(key);
					}

					succ();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				try {
					tst_n = 4;
					test_header("encrypt_string::encrypt_string()", tst_n);

					string psw = "abcd";
					encr::encrypt_string es{psw};
					out << "user = " << psw << '\n';
					string empty_key;
					string enc = es(empty_key);
					out << "password = " << enc << '\n';

					fails();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}
		}
	}

	if (decrypt_string) {
		tst_n = 0;

		if (decrypt_string_decrypt_string || decrypt_string_file_operator) {
			{
				tst_n = 1;
				try {
					test_header("decrypt_string::decrypt_string()", tst_n);

					string psw = "Administrator";
					string key = "m90";
					encr::encrypt_string es{psw};
					encr::decrypt_string ds{es(key), key};

					out << "password = " << psw << '\n';
					out << "key = " << key << '\n';
					out << "encryption password = " << es(key) << '\n';
					out << "password = " << ds() << '\n';

					if (ds() == psw)
						succ();
					else
						fails();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 2;
				bool test_result = true;
				try {
					test_header("decrypt_string::decrypt_string()", tst_n);

					const int psw_count{100'000};
					for (int i = 0; i < psw_count; ++i) {
						int max_ascii{122}, min_ascii{33};

						string password;
						int pass_length = std::rand() % 50 + 1;
						for (int j = 0; j < pass_length; ++j)
							password.push_back(static_cast<char>(
								min_ascii +
								(std::rand() % (max_ascii - min_ascii + 1))));

						string key;
						int key_len = 16;
						for (int j = 0; j < key_len; ++j)
							key.push_back(static_cast<char>(
								min_ascii +
								(std::rand() % (max_ascii - min_ascii + 1))));

						encr::encrypt_string e{password};
						string encryption{e(key)};
						encr::decrypt_string d{encryption, key};

						if (password != d()) test_result = false;

						out << "Password = " << password << '\n';
						out << "Key = " << key << '\n';
						out << "Encryption = " << encryption << '\n';
						out << "Decryption = " << d() << '\n';
						out << "---\n";
					}

					if (test_result)
						succ();
					else
						fails();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 3;
				bool test_result = true;
				try {
					test_header("decrypt_string::decrypt_string()", tst_n);

					string password{"admin1234"};
					string key = password;

					encr::encrypt_string e{password};
					string encryption{e(key)};
					encr::decrypt_string d{encryption, key};

					if (password != d()) test_result = false;

					out << "Password = " << password << '\n';
					out << "Key = " << key << '\n';
					out << "Encryption = " << encryption << '\n';
					out << "Decryption = " << d() << '\n';
					out << "---\n";

					if (test_result)
						succ();
					else
						fails();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 4;
				bool test_result = true;
				try {
					test_header("decrypt_string::decrypt_string()", tst_n);

					string password{"1234admin"};
					string key = password;

					encr::encrypt_string e{password};
					string encryption{e(key)};
					encr::decrypt_string d{encryption, key};

					if (password != d()) test_result = false;

					out << "Password = " << password << '\n';
					out << "Key = " << key << '\n';
					out << "Encryption = " << encryption << '\n';
					out << "Decryption = " << d() << '\n';
					out << "---\n";

					if (test_result)
						succ();
					else
						fails();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 4;
				bool test_result = true;
				try {
					test_header("decrypt_string::decrypt_string()", tst_n);

					string password{"admin1234"};
					string key = password;

					encr::encrypt_string e{password};
					string encryption{e(key)};

					std::ostringstream oss;
					for (const char c : encryption)
						oss << std::hex << std::setw(sizeof(unsigned short))
							<< std::setfill('0')
							<< static_cast<unsigned short>(c) << ' ';

					string encrypted_hex;
					std::istringstream iss{oss.str()};
					iss.setf(std::ios::hex, std::ios::basefield);
					for (unsigned short v; iss >> v;)
						encrypted_hex.push_back(
							static_cast<string::value_type>(v));

					encr::decrypt_string d{encrypted_hex, key};

					if (password != d()) test_result = false;

					out << "Password = " << password << '\n';
					out << "Key = " << key << '\n';
					out << "Encryption = " << encryption << '\n';
					out << "Encrypted_hex = " << encrypted_hex << '\n';

					if (test_result)
						succ();
					else
						fails();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				try {
					tst_n = 5;
					test_header("decrypt_string::decrypt_string()", tst_n);

					string psw = "abcd";
					string empty_key;
					encr::decrypt_string ds{psw, empty_key};
					out << "user = " << psw << '\n';
					string dec = ds();
					out << "password = " << dec << '\n';

					fails();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				try {
					tst_n = 6;
					test_header("decrypt_string::decrypt_string()", tst_n);

					string psw = "";
					string empty_key{"admin"};
					encr::decrypt_string ds{psw, empty_key};
					out << "user = " << psw << '\n';
					string dec = ds();
					out << "password = " << dec << '\n';

					fails();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}
		}
	}

	if (encrypt_string_file) {
		tst_n = 0;

		if (encrypt_string_file_encrypt_string_file ||
			encrypt_string_file_operator) {
			{
				tst_n = 1;
				try {
					test_header("encrypt_string_file::encrypt_string_file()",
								tst_n);

					encr::encrypt_string_file ef{"file_to_encrypt.txt",
												 "file_to_encrypt.txt.enc"};
					string key = "administrator";
					ef(key);
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 2;
				try {
					test_header("encrypt_string_file::encrypt_string_file()",
								tst_n);
					test_desc("Encrypting an empty file.");

					string ifl = "tmp_file_to_encrypt.txt";
					string ofl = ifl + ".enc";
					{
						std::ofstream{ifl};
						std::ofstream{ofl};
					}

					encr::encrypt_string_file ef{ifl, ofl};
					string key = "administrator";
					ef(key);

					int num_lines{0};
					{
						std::ifstream ifl_ifs{ifl};
						std::ifstream ofl_ifs{ofl};

						for (string ln; std::getline(ifl_ifs, ln);) ++num_lines;
						for (string ln; std::getline(ofl_ifs, ln);) ++num_lines;
					}

					if (!num_lines)
						succ();
					else
						fails();

					std::remove(ifl.c_str());
					std::remove(ofl.c_str());

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 3;
				bool test_result = true;
				try {
					test_header("encrypt_string_file::encrypt_string_file()",
								tst_n);
					test_desc("I/O files has same number of lines.");

					string ifl = "file_to_encrypt.txt";
					string ofl = ifl + ".enc";

					encr::encrypt_string_file ef{ifl, ofl};
					string key = "administrator";
					ef(key);

					{
						int ifl_num_lines{0};
						int ofl_num_lines{0};
						std::ifstream ifl_ifs{ifl};
						std::ifstream ofl_ifs{ofl};

						for (string ln; std::getline(ifl_ifs, ln);)
							++ifl_num_lines;
						for (string ln; std::getline(ofl_ifs, ln);)
							++ofl_num_lines;

						if (ifl_num_lines == ofl_num_lines) test_result = true;
					}

					if (test_result)
						succ();
					else
						fails();

					std::remove(ofl.c_str());

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 4;
				try {
					test_header("encrypt_string_file::encrypt_string_file()",
								tst_n);
					test_desc("Try to encrypt a file that does not exist.");

					string ifl = "file_to_encrypt_do_not_exist.txt";
					string ofl = ifl + ".enc";

					encr::encrypt_string_file ef{ifl, ofl};
					string key = "administrator";
					ef(key);

					fails();

				} catch (encr::encrypt_string_file_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 5;

				string empty_org_fl = "empty_file_to_encrypt.txt";
				string empty_enc_fl = "empty_file_to_encrypt.txt.enc";

				auto cleanup = [&empty_org_fl, &empty_enc_fl]() {
					std::remove(empty_org_fl.c_str());
					std::remove(empty_enc_fl.c_str());
				};

				try {
					test_header("encrypt_string_file::encrypt_string_file()",
								tst_n);
					test_desc("Encrypt an empty file.");

					{ std::ofstream{empty_org_fl}; }
					{ std::ofstream{empty_enc_fl}; }

					string key = "admin";
					encr::encrypt_string_file ef{empty_org_fl, empty_enc_fl};
					ef(key);

					succ();

					cleanup();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				}
			}
		}

		if (encrypt_file) {
			{
				tst_n = 1;
				string empty_org_fl = "single_call_file_to_encrypt.txt";

				try {
					test_header("encr::encrypt_file()", tst_n);
					test_desc(
						"Encrypt file with a single call to free function.");

					std::ofstream ofs{empty_org_fl}; 
					ofs << "encr::encrypt_file() copy\n";
					ofs << "new line of encrypt file read write\n";
					ofs.close();

					encr::encrypt_file(empty_org_fl, "admin");

					succ();

					std::remove(empty_org_fl.c_str());
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					std::remove(empty_org_fl.c_str());
				}
			}
			
			{
				tst_n = 2;
				string org_fl = "single_call_file_to_encrypt.txt";
				string org_fl_copy = "single_call_file_to_encrypt.txt.enc";
				bool test_result{false};
				try {
					test_header("encr::encrypt_file()", tst_n);
					test_desc("Encrypt file is equal to encrypt_string_file class?");

					std::ofstream ofs{org_fl}; 
					ofs << "encr::encrypt_file() copy\n";
					ofs << "new line of encrypt file read write\n";
					ofs.close();

					encr::encrypt_string_file esf{org_fl, org_fl_copy};
					esf("admin");

					encr::encrypt_file(org_fl, "admin");
							
					std::ifstream org_fl_ifs{org_fl};
					std::ifstream org_fl_copy_ifs{org_fl_copy};
					org_fl_ifs >> std::noskipws;
					org_fl_copy_ifs >> std::noskipws;
					test_result = std::equal(
						std::istream_iterator<char>(org_fl_ifs),
						std::istream_iterator<char>(),
						std::istream_iterator<char>(org_fl_copy_ifs));
					org_fl_ifs.close();
					org_fl_copy_ifs.close();
					
					if (test_result)
						succ();
					else
						fails();

					std::remove(org_fl.c_str());
					std::remove(org_fl_copy.c_str());
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					std::remove(org_fl.c_str());
					std::remove(org_fl_copy.c_str());
				}
				
			}
			
			{
				tst_n = 3;
				string org_fl = "do_not_exist_file.txt";
				try {
					test_header("encr::encrypt_file()", tst_n);
					test_desc("Encrypt a file that do not exist.");

					encr::encrypt_file(org_fl, "admin");
							
					fails();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
				}
				
			}
		}
	}

	if (decrypt_string_file) {
		tst_n = 0;

		if (decrypt_string_file_decrypt_string_file ||
			decrypt_string_file_operator) {
			{
				tst_n = 1;
				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);

					encr::encrypt_string_file ef{"file_to_encrypt.txt",
												 "file_to_encrypt.txt.enc"};
					string key = "administrator";
					ef(key);

					encr::decrypt_string_file df{"file_to_encrypt.txt.enc",
												 "file_to_encrypt.txt.dec",
												 key};
					df();

					succ();

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 2;
				bool test_result = "false";
				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc("Decrypting an empty file.");

					string ifl = "tmp_file_to_encrypt.enc";
					string ofl = ifl + ".dec";
					{
						std::ofstream{ifl};
						std::ofstream{ofl};
					}

					string key = "administrator";
					encr::decrypt_string_file df{ifl, ofl, key};
					df();

					{
						std::ifstream ifl_ifs{ifl};
						std::ifstream ofl_ifs{ofl};

						int num_lines{0};
						for (string ln; std::getline(ifl_ifs, ln);) ++num_lines;
						for (string ln; std::getline(ofl_ifs, ln);) ++num_lines;
						if (!num_lines) test_result = true;
					}

					if (test_result)
						succ();
					else
						fails();

					std::remove(ifl.c_str());
					std::remove(ofl.c_str());

				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 3;
				bool test_result = "false";

				string orig_fl = "file_to_encrypt.txt";
				string enc_fl = "file_to_encrypt.txt.enc";
				string dec_fl = "file_to_encrypt.txt.dec";
				auto cleanup = [&enc_fl, &dec_fl]() {
					std::remove(enc_fl.c_str());
					std::remove(dec_fl.c_str());
				};
				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc("Are original file and decrypted file equal?");

					string key = "administrator";
					encr::decrypt_string_file df{enc_fl, dec_fl, key};
					df();

					{
						std::ifstream orig_fl_ifs{orig_fl};
						std::ifstream dec_fl_ifs{dec_fl};

						bool files_are_equal{true};
						for (string orig_fl_ln;
							 std::getline(orig_fl_ifs, orig_fl_ln);) {
							string dec_fl_ln;
							std::getline(dec_fl_ifs, dec_fl_ln);

							if (orig_fl_ln != dec_fl_ln) {
								files_are_equal = false;
								break;
							}
						}

						if (orig_fl_ifs.eof() && files_are_equal)
							test_result = true;
					}

					if (test_result)
						succ();
					else
						fails();

					cleanup();

				} catch (encr::encrypt_string_file_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (encr::decrypt_string_file_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				}
			}

			{
				tst_n = 4;
				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc("Try to decrypt a file that does not exist.");

					string ifl = "file_to_decrypt_do_not_exist.txt";
					string ofl = ifl + ".enc";

					string key = "administrator";
					encr::decrypt_string_file df{ifl, ofl, key};
					df();

					fails();

				} catch (encr::decrypt_string_file_error& e) {
					out << e.what() << '\n';
					succ();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
				}
			}

			{
				tst_n = 5;
				bool test_result = false;

				string orig_fl = "file_to_encrypt.jpg";
				string enc_fl = "file_to_encrypt.jpg.enc";
				string dec_fl = "file_to_encrypt.jpg.dec";

				auto cleanup = [&enc_fl, &dec_fl]() {
					std::remove(enc_fl.c_str());
					std::remove(dec_fl.c_str());
				};
				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc(
						"Cannot be encrypt and decrypt image files(.jpg)");

					string key = "administrator";
					encr::encrypt_string_file ef{orig_fl, enc_fl};
					ef(key);
					encr::decrypt_string_file df{enc_fl, dec_fl, key};
					df();

					{
						std::ifstream orig_fl_ifs{
							orig_fl, std::ios::binary | std::ios::ate};
						std::ifstream dec_fl_ifs{
							dec_fl, std::ios::binary | std::ios::ate};

						if (orig_fl_ifs && dec_fl_ifs) {
							if (orig_fl_ifs.tellg() == dec_fl_ifs.tellg()) {
								orig_fl_ifs.seekg(0, std::ifstream::beg);
								dec_fl_ifs.seekg(0, std::ifstream::beg);

								test_result = std::equal(
									std::istream_iterator<char>(orig_fl_ifs),
									std::istream_iterator<char>(),
									std::istream_iterator<char>(dec_fl_ifs));
							}
						}
					}

					if (test_result)
						fails();
					else
						succ();

					cleanup();

				} catch (encr::encrypt_string_file_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (encr::decrypt_string_file_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				}
			}

			{
				tst_n = 6;

				string orig_fl = "file_to_encrypt.txt";
				string enc_fl = "file_to_encrypt.txt.enc";
				string dec_fl = "file_to_encrypt.txt.dec";
				auto cleanup = [&enc_fl, &dec_fl]() {
					std::remove(enc_fl.c_str());
					std::remove(dec_fl.c_str());
				};

				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc("Encrypt a file with an empty key?");

					string key = "";
					encr::encrypt_string_file ef{orig_fl, enc_fl};
					ef(key);
					encr::decrypt_string_file df{enc_fl, dec_fl, key};
					df();

					fails();

					cleanup();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					succ();
					cleanup();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				}
			}

			{
				tst_n = 7;

				string empty_org_fl = "empty_file_to_decrypt.txt.enc";
				string empty_enc_fl = "empty_file_to_decrypt.txt.dec";

				auto cleanup = [&empty_org_fl, &empty_enc_fl]() {
					std::remove(empty_org_fl.c_str());
					std::remove(empty_enc_fl.c_str());
				};

				try {
					test_header("decrypt_string_file::decrypt_string_file()",
								tst_n);
					test_desc("Dencrypt an empty file.");

					{ std::ofstream{empty_org_fl}; }
					{ std::ofstream{empty_enc_fl}; }

					string key = "admin";
					encr::decrypt_string_file dsf{empty_org_fl, empty_enc_fl,
												  key};
					dsf();

					succ();

					cleanup();

				} catch (encr::encryption_error& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				} catch (std::exception& e) {
					out << e.what() << '\n';
					fails();
					cleanup();
				}
			}
		}
	}
	
	if (decrypt_file) {

		{
			tst_n = 1;
			string org_fl = "single_call_file_to_encrypt.txt";
			string org_fl_tmp = "single_call_file_to_encrypt_tmp.txt";
			bool test_result{false};
			try {
				test_header("encr::decrypt_file()", tst_n);
				test_desc(
					"decrypt a file with a single call to free function.");

				std::ofstream ofs{org_fl}; 
				ofs << "encr::decrypt_file() copy\n";
				ofs << "new line of encrypt file read write\n";
				ofs.close();

				std::ifstream ifs{org_fl};
				std::noskipws(ifs);
				std::ofstream ofs_tmp{org_fl_tmp};
				std::copy(std::istream_iterator<char>{ifs},
						  std::istream_iterator<char>{},
						  std::ostream_iterator<char>{ofs_tmp});
				ifs.close();
				ofs_tmp.close();

				encr::encrypt_file(org_fl, "admin");

				encr::decrypt_file(org_fl, "admin");

				std::ifstream org_fl_ifs{org_fl};
				std::ifstream org_fl_tmp_ifs{org_fl_tmp};
				test_result = std::equal(std::istream_iterator<char>{org_fl_ifs},
						                 std::istream_iterator<char>{},
										 std::istream_iterator<char>{org_fl_tmp_ifs});
				org_fl_ifs.close();
				org_fl_tmp_ifs.close();

				if (test_result)
					succ();
				else
					fails();

				std::remove(org_fl.c_str());
				std::remove(org_fl_tmp.c_str());
				
			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
				std::remove(org_fl.c_str());
				std::remove(org_fl_tmp.c_str());
			}
		}
		
		{
			tst_n = 2;
			string org_fl = "single_call_file_to_encrypt.txt";
			string org_fl_copy = "single_call_file_to_encrypt.txt.enc";
			string org_fl_copy_tmp = "single_call_file_to_encrypt_tmp.txt.enc";
			bool test_result{false};
			try {
				test_header("encr::decrypt_file()", tst_n);
				test_desc("Decrypt file is equal to encrypt_string_file class?");

				std::ofstream ofs{org_fl}; 
				ofs << "encr::encrypt_file() copy\n";
				ofs << "new line of encrypt file read write\n";
				ofs.close();

				encr::encrypt_string_file esf{org_fl, org_fl_copy};
				esf("admin");
				encr::decrypt_string_file dsf{org_fl_copy, org_fl_copy_tmp, "admin"};
				dsf();

				encr::decrypt_file(org_fl_copy, "admin");

				std::ifstream org_fl_copy_ifs{org_fl_copy};
				std::ifstream org_fl_copy_tmp_ifs{org_fl_copy_tmp};
				std::noskipws(org_fl_copy_ifs);
				std::noskipws(org_fl_copy_tmp_ifs);
				test_result = std::equal(
					std::istream_iterator<char>(org_fl_copy_ifs),
					std::istream_iterator<char>(),
					std::istream_iterator<char>(org_fl_copy_tmp_ifs));
				org_fl_copy_ifs.close();
				org_fl_copy_tmp_ifs.close();
				
				if (test_result)
					succ();
				else
					fails();

				std::remove(org_fl.c_str());
				std::remove(org_fl_copy.c_str());
				std::remove(org_fl_copy_tmp.c_str());
			} catch (std::exception& e) {
				out << e.what() << '\n';
				fails();
				std::remove(org_fl.c_str());
				std::remove(org_fl_copy.c_str());
				std::remove(org_fl_copy_tmp.c_str());
			}
		}
		
		{
			tst_n = 3;
			string org_fl = "do_not_exist_file.txt";
			try {
				test_header("encr::decrypt_file()", tst_n);
				test_desc("Decrypt a file that do not exist.");

				encr::decrypt_file(org_fl, "admin");
						
				fails();

			} catch (encr::encryption_error& e) {
				out << e.what() << '\n';
				succ();
			}
			
		}
		
	}

	std::cout << "End '" + lib_name + "' library test.\n";

	return 0;
}

void test_header(string t) {
	out << "*******************\n";
	out << t;
}

void test_header(string t, int n) {
	test_header(t);
	out << " test: " << n << '\n';
}

void test_desc(string d) { out << "Description: " + d + '\n'; }

void succ() { out << "-success.\n"; }
void fails(string r) {
	if (r.size()) out << r << '\n';
	out << "-failure.\n";
}

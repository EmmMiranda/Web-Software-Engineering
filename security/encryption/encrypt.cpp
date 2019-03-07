#include "encrypt.h"
#include <fstream>
#include <iomanip>
#include <iostream>
#include <iterator>
#include <sstream>
#include <stdexcept>

namespace sage_100::ik::rnd_exp_rpt::sec::encr {

	string encrypt::operator()(const string& key) const {
		const int char_count = 2 * sizeof(long);
		const int key_char_count = 2 * char_count;
		const char pad_char{'\0'};

		const string decrypt_dat{pad_to_multiple(dat, char_count, pad_char)};
		string encrypt_dat(decrypt_dat.size(), pad_char);
		const string encrypt_key{
			pad_to_multiple(key, key_char_count, pad_char)};

		auto* k = reinterpret_cast<const unsigned long*>(encrypt_key.data());

		for (string::size_type pos = 0; pos < decrypt_dat.size();
			 pos += char_count) {
			auto* decr = reinterpret_cast<const unsigned long*>(
				decrypt_dat.data() + pos);

			auto* encr = reinterpret_cast<unsigned long*>(
				const_cast<char*>(encrypt_dat.data() + pos));

			tea_encrypt(decr, encr, k);
		}

		return encrypt_dat;
	}

	void encrypt::tea_encrypt(const unsigned long* const d,
							  unsigned long* const e,
							  const unsigned long* const k) const {
		static_assert(sizeof(long) == 4,
					  "size of build-in type 'long' is wrong for TEA "
					  "encryption algorithm");

		unsigned long y = d[0];
		unsigned long z = d[1];
		unsigned long sum = 0;
		const unsigned long delta = 0x9E3779B9;

		for (unsigned long n = 32; n-- > 0;) {
			y += (z << 4 ^ z >> 5) + (z ^ sum) + k[sum & 3];
			sum += delta;
			z += (y << 4 ^ y >> 5) + (y ^ sum) + k[sum >> 11 & 3];
		}

		e[0] = y;
		e[1] = z;
	}

	decrypt::decrypt(const string& ee, const string& key) {
		const int char_count = 2 * sizeof(long);
		const int key_char_count = 2 * char_count;
		const char pad_char{'\0'};

		const string encrypt_dat{pad_to_multiple(ee, char_count, pad_char)};
		string decrypt_dat(encrypt_dat.size(), pad_char);
		const string decrypt_key{
			pad_to_multiple(key, key_char_count, pad_char)};

		auto* k = reinterpret_cast<const unsigned long*>(decrypt_key.data());

		for (string::size_type pos = 0; pos < encrypt_dat.size();
			 pos += char_count) {
			auto* encr = reinterpret_cast<const unsigned long*>(
				encrypt_dat.data() + pos);

			auto* decr = reinterpret_cast<unsigned long*>(
				const_cast<char*>(decrypt_dat.data() + pos));

			tea_decrypt(encr, decr, k);
		}

		for (auto c : decrypt_dat)
			if (c != pad_char) dat.push_back(c);
	}

	void decrypt::tea_decrypt(const unsigned long* const e,
							  unsigned long* const d,
							  const unsigned long* const k) const {
		static_assert(sizeof(long) == 4,
					  "size of build-in type 'long' is wrong for TEA "
					  "decryption algorithm");

		unsigned long y = e[0];
		unsigned long z = e[1];
		unsigned long sum = 0xC6EF3720;
		const unsigned long delta = 0x9E3779B9;

		for (unsigned long n = 32; n-- > 0;) {
			z -= (y << 4 ^ y >> 5) + (y ^ sum) + k[sum >> 11 & 3];
			sum -= delta;
			y -= (z << 4 ^ z >> 5) + (z ^ sum) + k[sum & 3];
		}

		d[0] = y;
		d[1] = z;
	}

	/**
	 * @brief Compute a value that represent the closest multiplier of a value
	 * @param value Contain the value for which we wants to find the closest
	 *        multiplier.
	 * @param multiple multipler to be used.
	 * @return the nearest positive multipler of the argument value.
	 *
	 */
	int round_to_multiple(int value, int multiple) {
		if (multiple == 0)
			throw std::runtime_error{"'multiple' parameter cannot be zero."};

		if (value < 0)
			throw std::runtime_error{"'value' paramater cannot be negative"};

		int remainder = value % multiple;

		if (remainder == 0) return value;

		return value + multiple - remainder;
	}

	/**
	 * @brief Pad string to a multiplier length with a specific character.
	 * @param dat String object to pad.
	 * @param multiple Multiple used to compute the length of the pad string.
	 * @param ch Character to used to pad.
	*/
	string pad_to_multiple(const string dat, int multiple, char ch) {
		int pad_length{round_to_multiple(dat.size(), multiple)};

		string wrk_dat{dat};
		wrk_dat.resize(pad_length, ch);

		return wrk_dat;
	}

	encrypt_string::encrypt_string(const string& str) : m_str{str} {
		if (m_str.empty())
			throw encryption_error{"encrypt_string", "encrypt_string()",
								   "String to encrypt cannot be empty."};
	}

	string encrypt_string::operator()(const string& key) const {
		if (key.empty())
			throw encryption_error{"encrypt_string", "operator()",
								   "Encryption key cannot be empty."};

		const int char_count = 2 * sizeof(long);
		const int key_char_count = 2 * char_count;
		const char pad_char{'\0'};

		const string decrypt_dat{pad_to_multiple(m_str, char_count, pad_char)};
		string encrypt_dat(decrypt_dat.size(), pad_char);
		const string encrypt_key{
			pad_to_multiple(key, key_char_count, pad_char)};

		auto* k = reinterpret_cast<const unsigned long*>(encrypt_key.data());

		for (string::size_type pos = 0; pos < decrypt_dat.size();
			 pos += char_count) {
			auto* decr = reinterpret_cast<const unsigned long*>(
				decrypt_dat.data() + pos);

			auto* encr = reinterpret_cast<unsigned long*>(
				const_cast<char*>(encrypt_dat.data() + pos));

			tea_encrypt(decr, encr, k);
		}

		return encrypt_dat;
	}

	decrypt_string::decrypt_string(const string& str, const string& key) {
		if (str.empty())
			throw encryption_error{"decrypt_string", "decrypt_string()",
								   "String to decrypt cannot be empty."};

		if (key.empty())
			throw encryption_error{"decrypt_string", "decrypt_string()",
								   "Decryption key cannot be emtpty."};

		const int char_count = 2 * sizeof(long);
		const int key_char_count = 2 * char_count;
		const char pad_char{'\0'};

		const string encrypt_dat{pad_to_multiple(str, char_count, pad_char)};
		string decrypt_dat(encrypt_dat.size(), pad_char);
		const string decrypt_key{
			pad_to_multiple(key, key_char_count, pad_char)};

		auto* k = reinterpret_cast<const unsigned long*>(decrypt_key.data());

		for (string::size_type pos = 0; pos < encrypt_dat.size();
			 pos += char_count) {
			auto* encr = reinterpret_cast<const unsigned long*>(
				encrypt_dat.data() + pos);

			auto* decr = reinterpret_cast<unsigned long*>(
				const_cast<char*>(decrypt_dat.data() + pos));

			tea_decrypt(encr, decr, k);
		}

		for (auto c : decrypt_dat)
			if (c != pad_char) m_str.push_back(c);
	}

	void encrypt_string_file::operator()(const string& key) const {
		std::ifstream ifs{m_ifile};
		if (!ifs)
			throw encrypt_string_file_error{
				"encrypt_string_file()",
				"Cannot open input file '" + m_ifile + "' for encryption."};

		std::ofstream ofs{m_ofile};
		if (!ofs)
			throw encrypt_string_file_error{
				"encrypt_string_file()",
				"Cannot create output file '" + m_ofile + "' for encryption."};

		for (string ln; std::getline(ifs, ln);)
			ofs << to_hexadecimal(encrypt_string{ln}(key)) << '\n';
	}

	void decrypt_string_file::operator()() const {
		try {
			std::ifstream ifs{m_ifile};
			if (!ifs)
				throw decrypt_string_file_error{
					"decrypt_string_file()",
					"Cannot open input file '" + m_ifile + "' for decryption."};

			std::ofstream ofs{m_ofile};
			if (!ofs)
				throw decrypt_string_file_error{
					"decrypt_string_file()", "Cannot create output file '" +
												 m_ofile + "' for decryption."};

			for (string ln; std::getline(ifs, ln);) {
				decrypt_string ds{to_character(ln), m_key};
				ofs << ds() << '\n';
			}
		} catch (to_character_error& e) {
			std::remove(m_ofile.c_str());
			throw;
		}
	}

	void tea_encrypt(const unsigned long* const d, unsigned long* const e,
					 const unsigned long* const k) {
		static_assert(sizeof(long) == 4,
					  "size of build-in type 'long' is wrong for TEA "
					  "encryption algorithm");

		unsigned long y = d[0];
		unsigned long z = d[1];
		unsigned long sum = 0;
		const unsigned long delta = 0x9E3779B9;

		for (unsigned long n = 32; n-- > 0;) {
			y += (z << 4 ^ z >> 5) + (z ^ sum) + k[sum & 3];
			sum += delta;
			z += (y << 4 ^ y >> 5) + (y ^ sum) + k[sum >> 11 & 3];
		}

		e[0] = y;
		e[1] = z;
	}

	void tea_decrypt(const unsigned long* const e, unsigned long* const d,
					 const unsigned long* const k) {
		static_assert(sizeof(long) == 4,
					  "size of build-in type 'long' is wrong for TEA "
					  "decryption algorithm");

		unsigned long y = e[0];
		unsigned long z = e[1];
		unsigned long sum = 0xC6EF3720;
		const unsigned long delta = 0x9E3779B9;

		for (unsigned long n = 32; n-- > 0;) {
			z -= (y << 4 ^ y >> 5) + (y ^ sum) + k[sum >> 11 & 3];
			sum -= delta;
			y -= (z << 4 ^ z >> 5) + (z ^ sum) + k[sum & 3];
		}

		d[0] = y;
		d[1] = z;
	}

	string to_hexadecimal(const string& char_str) {
		std::ostringstream hex_oss;
		for (const char c : char_str)
			hex_oss << std::hex << std::setw(sizeof(unsigned short) * 2)
					<< std::setfill('0') << static_cast<unsigned short>(c)
					<< ' ';

		return hex_oss.str();
	}

	string to_character(const string& hexadecimal_str) {
		string password;
		std::istringstream hex_iss{hexadecimal_str};
		hex_iss.setf(std::ios::hex, std::ios::basefield);
		for (unsigned short v; hex_iss >> v;)
			password.push_back(static_cast<string::value_type>(v));

		if (hex_iss.fail() && !hex_iss.eof())
			throw to_character_error(
				"Cannot convert string of hexadecimal "
				"values to a character string.");

		return password;
	}

	void encrypt_file(const string& file, const string& key) {
		string tmp_file{file + ".tmp"};

		std::ifstream ifs{file};
		if (!ifs)
			throw encryption_error{
				"encr", "encrypt_file",
				"Cannot open input file '" + file + "' for encryption."};

		std::ofstream ofs{tmp_file};
		if (!ofs)
			throw encryption_error{
				"encr", "encrypt_file",
				"Cannot open temp file '" + tmp_file + "' for encryption."};

		ifs >> std::noskipws;
		std::copy(std::istream_iterator<char>{ifs},
				  std::istream_iterator<char>{},
				  std::ostream_iterator<char>{ofs});

		ifs.close();
		ofs.close();

		sec::encr::encrypt_string_file esf{tmp_file, file};
		esf(key);

		std::remove(tmp_file.c_str());
	}

	void decrypt_file(const string& file, const string& key) {
		string tmp_file{file + ".tmp"};

		std::ifstream ifs{file};
		if (!ifs)
			throw encryption_error{
				"encr", "encrypt_file",
				"Cannot open input file '" + file + "' for encryption."};

		std::ofstream ofs{tmp_file};
		if (!ofs)
			throw encryption_error{
				"encr", "encrypt_file",
				"Cannot open temp file '" + tmp_file + "' for encryption."};

		ifs >> std::noskipws;
		std::copy(std::istream_iterator<char>{ifs},
				  std::istream_iterator<char>{},
				  std::ostream_iterator<char>{ofs});

		ifs.close();
		ofs.close();

		sec::encr::decrypt_string_file dsf{tmp_file, file, key};
		dsf();

		std::remove(tmp_file.c_str());
	}
}

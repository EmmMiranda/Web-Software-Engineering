/**
 * @file security/encryption/encrypt.h
 * @brief Implement encryption and decryption algorithms
 */

#ifndef __ENCRYPT_H__
#define __ENCRYPT_H__

#include <stdexcept>
#include <string>

namespace sage_100::ik::rnd_exp_rpt::sec::encr {

	using std::string;

	class encrypt {
	   public:
		explicit encrypt(const string& dd) : dat{dd} {}
		encrypt(const encrypt& e) : dat{e.dat} {}
		string operator()(const string& key) const;

	   private:
		void tea_encrypt(const unsigned long* const d, unsigned long* const e,
						 const unsigned long* const k) const;
		string dat;
	};

	class decrypt {
	   public:
		decrypt(const string& ee, const string& key);
		string operator()() const { return dat; }

	   private:
		void tea_decrypt(const unsigned long* const e, unsigned long* const d,
						 const unsigned long* const k) const;
		string dat;
	};

	int round_to_multiple(int value, int multiple);
	string pad_to_multiple(const string dat, int multiple, char ch);

	struct encryption_error : std::runtime_error {
		encryption_error(const string& m) : runtime_error::runtime_error{m} {}
		encryption_error(const string& c, const string& f, const string& m)
			: runtime_error::runtime_error{c + "::" + f + ": " + m} {}
	};

	class encrypt_string {
	   public:
		explicit encrypt_string(const string& str);
		encrypt_string(const encrypt_string& e) : encrypt_string{e.m_str} {}

		string operator()(const string& key) const;

	   private:
		string m_str;
	};

	class decrypt_string {
	   public:
		decrypt_string(const string& str, const string& key);

		string operator()() const { return m_str; }

	   private:
		string m_str;
	};

	struct encrypt_string_file_error : encryption_error {
		encrypt_string_file_error(const string& f, const string& m)
			: encryption_error::encryption_error("encrypt_string_file", f, m) {}
	};

	class encrypt_string_file {
	   public:
		encrypt_string_file(const string& ifile, const string& ofile)
			: m_ifile{ifile}, m_ofile{ofile} {};
		encrypt_string_file(const encrypt_string_file& e)
			: m_ifile{e.m_ifile}, m_ofile{e.m_ofile} {}

		/**
			@brief Encrypt the input file and store result on output file.
			@param key Encryption key.
			@throw encrypt_string_file_error if cannot open input or output
		   file.
		*/
		void operator()(const string& key) const;

	   private:
		string m_ifile;
		string m_ofile;
	};

	struct decrypt_string_file_error : encryption_error {
		decrypt_string_file_error(const string& f, const string& m)
			: encryption_error::encryption_error("decrypt_string_file", f, m) {}
	};

	class decrypt_string_file {
	   public:
		explicit decrypt_string_file(const string& ifile, const string& ofile,
									 const string& key)
			: m_ifile{ifile}, m_ofile{ofile}, m_key{key} {}

		/**
			@brief Dencrypt input file and store result on output file.
			@throw decrypt_string_file_error if cannot open input or output
		   file.
		*/
		void operator()() const;

	   private:
		string m_ifile;
		string m_ofile;
		string m_key;
	};

	/**
	 * @brief Implement Tiny Encryption Algorithm - (TEA) block cipher.
	 * @param d Pointer to an  array of eight character to encipher.
	 * @param e Pointer to an array of eight character to store enchiper values.
	 * @param k Pointer to an array of sixteen characters, that represents
	 * encryption key.
	 *
	 * The algorithm encypher an array of 2 unsigned long that are represented
	 * as one pair of 'four bytes values'(v[0],v[1]) or 64-bit values. The key
	 * is represented as an array of four unsigned longs or 120-bit values.
	 * The encryption occurs on 32 iterations. For reference see:
	 * Bjarne Stroustrup. Programming: Principles and Practice using C++, 2014.
	 *
	 */
	void tea_encrypt(const unsigned long* const d, unsigned long* const e,
					 const unsigned long* const k);
	void tea_decrypt(const unsigned long* const e, unsigned long* const d,
					 const unsigned long* const k);

	string to_hexadecimal(const string& char_str);

	struct to_character_error : encryption_error {
		explicit to_character_error(const string& m) : 
			encryption_error::encryption_error("encr", "to_character()", m) {}
	}; 
	string to_character(const string& hexadecimal_str);
	
	/** 
	 * @throw encryption_error if cannot open file to encrypt 
	 */
	void encrypt_file(const string& file, const string& key);
	void decrypt_file(const string& file, const string& key);
}

#endif

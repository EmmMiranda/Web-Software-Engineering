MINGWPTH = c:\MinGW
PRJPTH = e:\workspace\sage_100\infomedika_module\ik_random_export_report_540

CXXFLAGS += -g -Wall -O3 -std=c++17

# add filesystem library
BOOSTPTH = $(MINGWPTH)
CXXFLAGS += -I $(BOOSTPTH)\include
LIBS += -L $(BOOSTPTH)\lib -lboost_filesystem -lboost_system

RPTLPTH = ..\report\Report_launcher
CXXFLAGS += -I $(RPTLPTH)
LIBS += -L $(MINGWPTH)\lib -lole32

CXXFLAGS += -I encryption

# add security library
SECPTH = $(PRJPTH)\security
CXXFLAGS += -I $(SECPTH)

OBJS = tests.o $(SECPTH)\sage_100_security.o encryption\encrypt.o $(RPTLPTH)\rpt_launcher.o $(RPTLPTH)\INIReader.o $(RPTLPTH)\ini.o

tests.exe: $(OBJS)
	 g++ -o $@ $^ $(LIBS)

clean:
	del -f *.o tests.exe

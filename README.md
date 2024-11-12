# Part 1

### 完成功能
- 讀取、解析卡匣 Header

### 運行指令
```bash
dotnet run path/to/file.gb
```


### 運行結果
```bash
V 參數輸入正確
卡匣 ROM Size : 32768 bytes
Title                | DMG-ACID2     
CGB Flag             | 0                   
Manufacturer Code    |                     
Cartridge Type       | ROM ONLY            
ROM Size Value       | 0                   
ROM Size             | 32768 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 0                   
Header Check Sum     | 159                  PASSED
V 讀取卡匣成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
X CPU 尚未實現
X CPU 停止運行
```

---

# Part 2

### 完成功能
- 初步完成 CPU 指令讀取

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
```bash
參數輸入完成
卡匣 ROM Size : 32768 bytes
Title                | DMG-ACID2     
CGB Flag             | 0                   
Manufacturer Code    |                     
Cartridge Type       | ROM ONLY            
ROM Size Value       | 0                   
ROM Size             | 32768 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 0                   
Header Check Sum     | 159                  PASSED
卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
CPU 初始化成功
Bef : Current Opcode - 00 | PC : 0100
Aft : Current Opcode - 00 | PC : 0101
Execute 尚未實現
------------------
Bef : Current Opcode - 00 | PC : 0101
Aft : Current Opcode - C3 | PC : 0104
Execute 尚未實現
------------------
Bef : Current Opcode - C3 | PC : 0104
Unknown Instruction : CE
```

---

# Part 3

### 完成功能
- 初步完成 CPU 指令執行

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
```bash
參數輸入完成
卡匣 ROM Size : 32768 bytes
Title                | DMG-ACID2     
CGB Flag             | 0                   
Manufacturer Code    |                     
Cartridge Type       | ROM ONLY            
ROM Size Value       | 0                   
ROM Size             | 32768 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 0                   
Header Check Sum     | 159                  PASSED
卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
CPU 初始化成功
PC: 0100 NOP (00, C3, 50) A:01 B:00 C:00
PC: 0101 JP (C3, 50, 01) A:01 B:00 C:00
PC: 0150 DI (F3, 31, FF) A:01 B:00 C:00
Unknown Instruction : 31
```

---

# Part 4

### 完成功能
- 完成 CPU LD 指令執行

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
本例使用 `mem_timing.gb`
```bash
V 參數輸入完成
卡匣 ROM Size : 65536 bytes
Title                | MEM_TIMING         
CGB Flag             | 128                 
Manufacturer Code    |                 
Cartridge Type       | MBC1                
ROM Size Value       | 1                   
ROM Size             | 65536 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 1                   
Header Check Sum     | 95                   PASSED
V 卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
PC: 0100 NOP (00, C3, 37) A:00 B:00 C:00
PC: 0101 JP (C3, 37, 06) A:00 B:00 C:00
PC: 0637 JP (C3, 30, 04) A:00 B:00 C:00
PC: 0430 DI (F3, 31, FF) A:00 B:00 C:00
PC: 0431 LD (31, FF, DF) A:00 B:00 C:00
PC: 0434 LD (EA, 00, D6) A:00 B:00 C:00
BusWrite() 尚未實現 - address:D600
PC: 0437 LD (3E, 00, E0) A:00 B:00 C:00
找不到 opcode 對應的 Instruction : E0
```


---

# Part 5

### 完成功能
- 完成部份 Memory Mapping 

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
本例使用 `cpu_instrs.gb`
```bash
V 參數輸入完成
卡匣 ROM Size : 65536 bytes
Title                | CPU_INSTRS         
CGB Flag             | 128                 
Manufacturer Code    |                 
Cartridge Type       | MBC1                
ROM Size Value       | 1                   
ROM Size             | 65536 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 1                   
Header Check Sum     | 59                   PASSED
V 卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
PC: 0100 NOP (00, C3, 37)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0101 JP (C3, 37, 06)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0637 JP (C3, 30, 04)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0430 DI (F3, 31, FF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0431 LD (31, FF, DF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0434 LD (EA, 00, D6)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0437 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0439 LDH (E0, 07, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 043B LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 043D LDH (E0, 0F, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 043F LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0441 LDH (E0, FF, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 0443 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0445 LDH (E0, 26, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 0447 LD (3E, 80, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0449 LDH (E0, 26, 3E)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044B LD (3E, FF, E0)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044D LDH (E0, 25, 3E)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044F LD (3E, 77, E0)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0451 LDH (E0, 24, 21)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF77 - 尚未實現PC: 0453 LD (21, 8F, 0B)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF
找不到 opcode 對應的 Instruction : CD
```


---

# Part 6

### 完成功能
- 完成 POP, PUSH, JP, JR, CALL, RST, RET, RETI

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb, dmg-acid2.gb
```


### 運行結果
本例使用 `cpu_instrs.gb`
```bash
V 參數輸入完成
卡匣 ROM Size : 65536 bytes
Title                | CPU_INSTRS         
CGB Flag             | 128                 
Manufacturer Code    |                 
Cartridge Type       | MBC1                
ROM Size Value       | 1                   
ROM Size             | 65536 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 1                   
Header Check Sum     | 59                   PASSED
V 卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
PC: 0100 NOP (00, C3, 37)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0101 JP (C3, 37, 06)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0637 JP (C3, 30, 04)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0430 DI (F3, 31, FF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0431 LD (31, FF, DF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000
PC: 0434 LD (EA, 00, D6)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0437 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0439 LDH (E0, 07, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 043B LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 043D LDH (E0, 0F, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 043F LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0441 LDH (E0, FF, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 0443 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0445 LDH (E0, 26, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現PC: 0447 LD (3E, 80, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0449 LDH (E0, 26, 3E)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044B LD (3E, FF, E0)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044D LDH (E0, 25, 3E)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 044F LD (3E, 77, E0)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0451 LDH (E0, 24, 21)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF77 - 尚未實現PC: 0453 LD (21, 8F, 0B)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF
PC: 0456 CALL (CD, A3, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFF
PC: 02A3 LD (7D, EA, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02A4 LD (EA, 02, D6)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02A7 LD (7C, EA, 03)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02A8 LD (EA, 03, D6)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02AB JR (18, 04, 3E)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02B1 LD (3E, C3, EA)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02B3 LD (EA, 01, D6)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 02B6 RET (C9, F5, FE)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 0459 CALL (CD, 8E, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFF
PC: 038E PUSH (E5, CD, 7B)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD
PC: 038F CALL (CD, 7B, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB
PC: 037B POP (E1, E5, F5)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFF9
PC: 037C PUSH (E5, F5, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFFB
PC: 037D PUSH (F5, 23, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFF9
找不到 opcode 對應的 Instruction : 23
```


---

# Part 7

### 完成功能
- 完成 ADD, INC, DEC, SUB, SBC, ADC

### 運行指令
```bash
dotnet run path/to/file.gb
```


### 運行結果
本例使用 `mem_timing.gb`
```bash
V 參數輸入完成
卡匣 ROM Size : 65536 bytes
Title                | MEM_TIMING         
CGB Flag             | 128                 
Manufacturer Code    |                 
Cartridge Type       | MBC1                
ROM Size Value       | 1                   
ROM Size             | 65536 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 1                   
Header Check Sum     | 95                   PASSED
V 卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
00000000 PC: 0100 NOP (00, C3, 37)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000001 PC: 0101 JP (C3, 37, 06)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000002 PC: 0637 JP (C3, 30, 04)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000003 PC: 0430 DI (F3, 31, FF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000004 PC: 0431 LD (31, FF, DF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000005 PC: 0434 LD (EA, 00, D6)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000006 PC: 0437 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000007 PC: 0439 LDH (E0, 07, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
00000008 PC: 043B LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000009 PC: 043D LDH (E0, 0F, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000A PC: 043F LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000B PC: 0441 LDH (E0, FF, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000C PC: 0443 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000D PC: 0445 LDH (E0, 26, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000E PC: 0447 LD (3E, 80, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000F PC: 0449 LDH (E0, 26, 3E)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000010 PC: 044B LD (3E, FF, E0)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000011 PC: 044D LDH (E0, 25, 3E)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000012 PC: 044F LD (3E, 77, E0)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000013 PC: 0451 LDH (E0, 24, 21)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF77 - 尚未實現
00000014 PC: 0453 LD (21, 8F, 0B)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000015 PC: 0456 CALL (CD, A3, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
00000016 PC: 02A3 LD (7D, EA, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000017 PC: 02A4 LD (EA, 02, D6)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000018 PC: 02A7 LD (7C, EA, 03)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000019 PC: 02A8 LD (EA, 03, D6)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001A PC: 02AB JR (18, 04, 3E)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001B PC: 02B1 LD (3E, C3, EA)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001C PC: 02B3 LD (EA, 01, D6)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001D PC: 02B6 RET (C9, F5, FE)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001E PC: 0459 CALL (CD, 8E, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
0000001F PC: 038E PUSH (E5, CD, 7B)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000020 PC: 038F CALL (CD, 7B, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000021 PC: 037B POP (E1, E5, F5)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFF9    Flag:----
00000022 PC: 037C PUSH (E5, F5, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFFB    Flag:----
00000023 PC: 037D PUSH (F5, 23, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFF9    Flag:----
00000024 PC: 037E INC (23, 23, 2A)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFF7    Flag:----
00000025 PC: 037F INC (23, 2A, EA)     AF:C300 BC:0000 DE:0000 HL:0393 SP:DFF7    Flag:----
00000026 PC: 0380 LD (2A, EA, 04)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000027 PC: 0381 LD (EA, 04, D6)     AF:FF00 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000028 PC: 0384 LD (7D, EA, 05)     AF:FF00 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000029 PC: 0385 LD (EA, 05, D6)     AF:9500 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002A PC: 0388 LD (7C, EA, 06)     AF:9500 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002B PC: 0389 LD (EA, 06, D6)     AF:0300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002C PC: 038C POP (F1, C9, E5)     AF:0300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002D PC: 038D RET (C9, E5, CD)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFF9    Flag:----
0000002E PC: 0392 JR (18, 02, FF)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFFB    Flag:----
0000002F PC: 0396 POP (E1, CD, 5D)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFFB    Flag:----
00000030 PC: 0397 CALL (CD, 5D, 02)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000031 PC: 025D JR (18, 00, 3E)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000032 PC: 025F LD (3E, FF, E0)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000033 PC: 0261 LDH (E0, C0, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000034 PC: 0263 LDH (E0, C1, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000035 PC: 0265 LDH (E0, C2, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000036 PC: 0267 LDH (E0, C3, C9)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000037 PC: 0269 RET (C9, F5, C5)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000038 PC: 039A RET (C9, CD, CA)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000039 PC: 045C CALL (CD, 79, 0B)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
0000003A PC: 0B79 CALL (CD, 4B, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000003B PC: 074B CALL (CD, EE, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
0000003C PC: 07EE PUSH (F5, CD, 3A)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF9    Flag:----
0000003D PC: 07EF CALL (CD, 3A, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF7    Flag:----
0000003E PC: 073A PUSH (C5, 01, 1E)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF5    Flag:----
0000003F PC: 073B LD (01, 1E, FB)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF3    Flag:----
00000040 PC: 073E INC (03, 78, B1)     AF:FF00 BC:FB1E DE:0000 HL:0B8F SP:DFF3    Flag:----
00000041 PC: 073F LD (78, B1, 28)     AF:FF00 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
找不到 opcode 對應的 Instruction : B1
```

如果想看到 Flag 變化，可以使用  `dotnet run 07-jr,jp,call,ret,rst.gb > test.txt`，部份輸出如下：
```bash
00011F45 PC: 0001 RET (C9, 00, 00)     AF:CD00 BC:0010 DE:C001 HL:4001 SP:0012    Flag:----
00011F46 PC: 0000 INC (3C, C9, 00)     AF:CD00 BC:0010 DE:C001 HL:4001 SP:0014    Flag:----
00011F47 PC: 0001 RET (C9, 00, 00)     AF:CE00 BC:0010 DE:C001 HL:4001 SP:0014    Flag:----
00011F48 PC: 0000 INC (3C, C9, 00)     AF:CE00 BC:0010 DE:C001 HL:4001 SP:0016    Flag:----
00011F49 PC: 0001 RET (C9, 00, 00)     AF:CF00 BC:0010 DE:C001 HL:4001 SP:0016    Flag:----
00011F4A PC: 0000 INC (3C, C9, 00)     AF:CF00 BC:0010 DE:C001 HL:4001 SP:0018    Flag:----
00011F4B PC: 0001 RET (C9, 00, 00)     AF:D020 BC:0010 DE:C001 HL:4001 SP:0018    Flag:--H-
00011F4C PC: C93C NOP (00, 00, 00)     AF:D020 BC:0010 DE:C001 HL:4001 SP:001A    Flag:--H-
00011F4D PC: C93D NOP (00, 00, 00)     AF:D020 BC:0010 DE:C001 HL:4001 SP:001A    Flag:--H-
00011F4E PC: C93E NOP (00, 00, 00)     AF:D020 BC:0010 DE:C001 HL:4001 SP:001A    Flag:--H-
00011F4F PC: C93F NOP (00, 00, 00)     AF:D020 BC:0010 DE:C001 HL:4001 SP:001A    Flag:--H-
```


---

# Part 8

### 完成功能
- 初步完成 CPU 指令執行

### 運行指令
```bash
dotnet run path/to/file.gb
# gb 檔案可選 :
# mem_timing.gb , cpu_instrs.gb
```


### 運行結果
本例使用 `mem_timing.gb`
```bash
V 參數輸入完成
卡匣 ROM Size : 65536 bytes
Title                | MEM_TIMING         
CGB Flag             | 128                 
Manufacturer Code    |                 
Cartridge Type       | MBC1                
ROM Size Value       | 1                   
ROM Size             | 65536 bytes
RAM Size Value       | 0 bytes
RAM Size             | 0 bytes
Old License Code     | 0                   
New License Code     |                     
Publisher            | None                
ROM Version          | 1                   
Header Check Sum     | 95                   PASSED
V 卡匣載入成功
V SDL 初始化成功
V TTF 初始化成功
V CPU 初始化成功
00000000 PC: 0100 NOP (00, C3, 37)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000001 PC: 0101 JP (C3, 37, 06)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000002 PC: 0637 JP (C3, 30, 04)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000003 PC: 0430 DI (F3, 31, FF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000004 PC: 0431 LD (31, FF, DF)     AF:0000 BC:0000 DE:0000 HL:0000 SP:0000    Flag:----
00000005 PC: 0434 LD (EA, 00, D6)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000006 PC: 0437 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000007 PC: 0439 LDH (E0, 07, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
00000008 PC: 043B LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000009 PC: 043D LDH (E0, 0F, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000A PC: 043F LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000B PC: 0441 LDH (E0, FF, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000C PC: 0443 LD (3E, 00, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000D PC: 0445 LDH (E0, 26, 3E)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF00 - 尚未實現
0000000E PC: 0447 LD (3E, 80, E0)     AF:0000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
0000000F PC: 0449 LDH (E0, 26, 3E)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000010 PC: 044B LD (3E, FF, E0)     AF:8000 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000011 PC: 044D LDH (E0, 25, 3E)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000012 PC: 044F LD (3E, 77, E0)     AF:FF00 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000013 PC: 0451 LDH (E0, 24, 21)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
BusWrite() - I/O Registers(0xFF00-0xFF7F):FF77 - 尚未實現
00000014 PC: 0453 LD (21, 8F, 0B)     AF:7700 BC:0000 DE:0000 HL:0000 SP:DFFF    Flag:----
00000015 PC: 0456 CALL (CD, A3, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
00000016 PC: 02A3 LD (7D, EA, 02)     AF:7700 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000017 PC: 02A4 LD (EA, 02, D6)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000018 PC: 02A7 LD (7C, EA, 03)     AF:8F00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000019 PC: 02A8 LD (EA, 03, D6)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001A PC: 02AB JR (18, 04, 3E)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001B PC: 02B1 LD (3E, C3, EA)     AF:0B00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001C PC: 02B3 LD (EA, 01, D6)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001D PC: 02B6 RET (C9, F5, FE)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000001E PC: 0459 CALL (CD, 8E, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
0000001F PC: 038E PUSH (E5, CD, 7B)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000020 PC: 038F CALL (CD, 7B, 03)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000021 PC: 037B POP (E1, E5, F5)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFF9    Flag:----
00000022 PC: 037C PUSH (E5, F5, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFFB    Flag:----
00000023 PC: 037D PUSH (F5, 23, 23)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFF9    Flag:----
00000024 PC: 037E INC (23, 23, 2A)     AF:C300 BC:0000 DE:0000 HL:0392 SP:DFF7    Flag:----
00000025 PC: 037F INC (23, 2A, EA)     AF:C300 BC:0000 DE:0000 HL:0393 SP:DFF7    Flag:----
00000026 PC: 0380 LD (2A, EA, 04)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000027 PC: 0381 LD (EA, 04, D6)     AF:FF00 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000028 PC: 0384 LD (7D, EA, 05)     AF:FF00 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
00000029 PC: 0385 LD (EA, 05, D6)     AF:9500 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002A PC: 0388 LD (7C, EA, 06)     AF:9500 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002B PC: 0389 LD (EA, 06, D6)     AF:0300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002C PC: 038C POP (F1, C9, E5)     AF:0300 BC:0000 DE:0000 HL:0395 SP:DFF7    Flag:----
0000002D PC: 038D RET (C9, E5, CD)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFF9    Flag:----
0000002E PC: 0392 JR (18, 02, FF)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFFB    Flag:----
0000002F PC: 0396 POP (E1, CD, 5D)     AF:C300 BC:0000 DE:0000 HL:0395 SP:DFFB    Flag:----
00000030 PC: 0397 CALL (CD, 5D, 02)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000031 PC: 025D JR (18, 00, 3E)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000032 PC: 025F LD (3E, FF, E0)     AF:C300 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000033 PC: 0261 LDH (E0, C0, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000034 PC: 0263 LDH (E0, C1, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000035 PC: 0265 LDH (E0, C2, E0)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000036 PC: 0267 LDH (E0, C3, C9)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000037 PC: 0269 RET (C9, F5, C5)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
00000038 PC: 039A RET (C9, CD, CA)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
00000039 PC: 045C CALL (CD, 79, 0B)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFF    Flag:----
0000003A PC: 0B79 CALL (CD, 4B, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFD    Flag:----
0000003B PC: 074B CALL (CD, EE, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFFB    Flag:----
0000003C PC: 07EE PUSH (F5, CD, 3A)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF9    Flag:----
0000003D PC: 07EF CALL (CD, 3A, 07)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF7    Flag:----
0000003E PC: 073A PUSH (C5, 01, 1E)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF5    Flag:----
0000003F PC: 073B LD (01, 1E, FB)     AF:FF00 BC:0000 DE:0000 HL:0B8F SP:DFF3    Flag:----
00000040 PC: 073E INC (03, 78, B1)     AF:FF00 BC:FB1E DE:0000 HL:0B8F SP:DFF3    Flag:----
00000041 PC: 073F LD (78, B1, 28)     AF:FF00 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
00000042 PC: 0740 OR (B1, 28, 06)     AF:FB00 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
00000043 PC: 0741 JR (28, 06, F0)     AF:FF00 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
00000044 PC: 0743 LDH (F0, 44, FE)     AF:FF00 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
BusRead() - I/O Registers(0xFF00-0xFF7F):FF44 - 尚未實現
00000045 PC: 0745 CP (FE, 90, 20)     AF:0000 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:----
00000046 PC: 0747 JR (20, F5, C1)     AF:0050 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:-N-C
00000047 PC: 083E LD (3E, 20, 21)     AF:0050 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:-N-C
00000048 PC: 0840 LD (21, 1A, D6)     AF:2050 BC:FB1F DE:0000 HL:0B8F SP:DFF3    Flag:-N-C
00000049 PC: 0843 LD (06, 14, 32)     AF:2050 BC:FB1F DE:0000 HL:D61A SP:DFF3    Flag:-N-C
0000004A PC: 0845 LD (32, 05, 20)     AF:2050 BC:141F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
0000004B PC: 0846 DEC (05, 20, FC)     AF:2050 BC:141F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
0000004C PC: 0847 JR (20, FC, 3E)     AF:2050 BC:131F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
0000004D PC: 0945 LD (06, 0C, 38)     AF:2050 BC:131F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
0000004E PC: 0947 JR (38, 00, 00)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
0000004F PC: 0949 NOP (00, 18, 18)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000050 PC: 094A JR (18, 18, 00)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000051 PC: 0964 NOP (00, 00, 7E)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000052 PC: 0965 NOP (00, 7E, 00)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000053 PC: 0966 LD (7E, 00, 00)     AF:2050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000054 PC: 0967 NOP (00, 00, 30)     AF:0050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000055 PC: 0968 NOP (00, 30, 18)     AF:0050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000056 PC: 0969 JR (30, 18, 0C)     AF:0050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000057 PC: 096B INC (0C, 06, 0C)     AF:0050 BC:0C1F DE:0000 HL:D619 SP:DFF3    Flag:-N-C
00000058 PC: 096C LD (06, 0C, 18)     AF:0030 BC:0C20 DE:0000 HL:D619 SP:DFF3    Flag:--HC
00000059 PC: 096E JR (18, 30, 00)     AF:0030 BC:0C20 DE:0000 HL:D619 SP:DFF3    Flag:--HC
0000005A PC: 09A0 NOP (00, 7E, 60)     AF:0030 BC:0C20 DE:0000 HL:D619 SP:DFF3    Flag:--HC
0000005B PC: 09A1 LD (7E, 60, 60)     AF:0030 BC:0C20 DE:0000 HL:D619 SP:DFF3    Flag:--HC
0000005C PC: 09A2 LD (60, 60, 7C)     AF:0030 BC:0C20 DE:0000 HL:D619 SP:DFF3    Flag:--HC
0000005D PC: 09A3 LD (60, 7C, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000005E PC: 09A4 LD (7C, 60, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000005F PC: 09A5 LD (60, 60, 7E)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000060 PC: 09A6 LD (60, 7E, 00)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000061 PC: 09A7 LD (7E, 00, 7E)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000062 PC: 09A8 NOP (00, 7E, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000063 PC: 09A9 LD (7E, 60, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000064 PC: 09AA LD (60, 60, 7C)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000065 PC: 09AB LD (60, 7C, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000066 PC: 09AC LD (7C, 60, 60)     AF:0030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000067 PC: 09AD LD (60, 60, 60)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000068 PC: 09AE LD (60, 60, 00)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
00000069 PC: 09AF LD (60, 00, 3E)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000006A PC: 09B0 NOP (00, 3E, 60)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000006B PC: 09B1 LD (3E, 60, 60)     AF:0C30 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000006C PC: 09B3 LD (60, 6E, 66)     AF:6030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000006D PC: 09B4 LD (6E, 66, 66)     AF:6030 BC:0C20 DE:0000 HL:0C19 SP:DFF3    Flag:--HC
0000006E PC: 09B5 LD (66, 66, 3E)     AF:6030 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:--HC
0000006F PC: 09B6 LD (66, 3E, 00)     AF:6030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000070 PC: 09B7 LD (3E, 00, 66)     AF:6030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000071 PC: 09B9 LD (66, 66, 66)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000072 PC: 09BA LD (66, 66, 7E)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000073 PC: 09BB LD (66, 7E, 66)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000074 PC: 09BC LD (7E, 66, 66)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000075 PC: 09BD LD (66, 66, 66)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000076 PC: 09BE LD (66, 66, 00)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000077 PC: 09BF LD (66, 00, 3C)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000078 PC: 09C0 NOP (00, 3C, 18)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
00000079 PC: 09C1 INC (3C, 18, 18)     AF:0030 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:--HC
0000007A PC: 09C2 JR (18, 18, 18)     AF:0110 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:---C
0000007B PC: 09DC LD (60, 60, 60)     AF:0110 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:---C
0000007C PC: 09DD LD (60, 60, 7E)     AF:0110 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:---C
0000007D PC: 09DE LD (60, 7E, 00)     AF:0110 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:---C
0000007E PC: 09DF LD (7E, 00, C6)     AF:0110 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:---C
0000007F PC: 09E0 NOP (00, C6, EE)     AF:0010 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:---C
00000080 PC: 09E1 ADD (C6, EE, FE)     AF:0010 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:---C
00000081 PC: 09E3 CP (FE, D6, C6)     AF:EE00 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:----
00000082 PC: 09E5 ADD (C6, C6, C6)     AF:EE40 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:-N--
00000083 PC: 09E7 ADD (C6, 00, 66)     AF:B430 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:--HC
00000084 PC: 09E9 LD (66, 76, 7E)     AF:B400 BC:0C20 DE:0000 HL:0C00 SP:DFF3    Flag:----
00000085 PC: 09EA HALT (76, 7E, 7E)     AF:B400 BC:0C20 DE:0000 HL:0000 SP:DFF3    Flag:----
Execute():未找到對應指令方法
```

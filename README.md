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
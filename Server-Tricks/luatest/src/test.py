with open('test.luac', 'rb') as f:
    n = 0
    s = f.read(1)
    while s:
        n = n + 1
        print('%02x' % ord(s), end=' ')
        if n % 16 == 0:
            print('')
        s = f.read(1)

print()

with open('test.luac', 'rb') as f:
    n = 0
    s = f.read(1)
    while s:
        n = n + 1
        if ord(s) < 32 or ord(s) > 127:
            print('.', end='')
        else:
            print(chr(ord(s)), end='')
        if n % 16 == 0:
            print('')
        s = f.read(1)

print()

##################################################
# https://github.com/lua/lua/blob/063d4e4543088e7a21965bda8ee5a0f952a9029e/ldump.c
##################################################

import struct

f = open('test.luac', 'rb')


def printString():
    size, = struct.unpack('B', f.read(1))
    size = eval(hex(size)) - 1  # 忽略'\0'
    # print('String长度: %d' % size )
    print('String值: %s' % struct.unpack(str(size) + 's', f.read(size)))


# dumpHeader
print('signature: %s' % struct.unpack('4s', f.read(4)))
print('version: %x' % struct.unpack('B', f.read(1)))
print('format: %x' % struct.unpack('B', f.read(1)))
print('data: %s' % struct.unpack('6s', f.read(6)))
print('size_int: %x' % struct.unpack('B', f.read(1)))
print('size_size_t: %x' % struct.unpack('B', f.read(1)))
print('size_Instruction: %x' % struct.unpack('B', f.read(1)))
print('size_lua_Integer: %x' % struct.unpack('B', f.read(1)))
print('size_lua_Number: %x' % struct.unpack('B', f.read(1)))
print('Int: %x' % struct.unpack('q', f.read(8)))
print('Num: %x' % struct.unpack('q', f.read(8)))

# dumpByte
print('sizeupvalues: %x' % struct.unpack('B', f.read(1)))

# dumpFunction
printString()
print('linedefined: %d' % struct.unpack('i', f.read(4)))
print('lastlinedefined: %d' % struct.unpack('i', f.read(4)))
print('numparams: %x' % struct.unpack('B', f.read(1)))
print('is_vararg: %x' % struct.unpack('B', f.read(1)))
print('maxstacksize: %x' % struct.unpack('B', f.read(1)))

# dumpCode
print('sizecode: %d' % struct.unpack('i', f.read(4)))
print('code 1: %s' % struct.unpack('4s', f.read(4)))
print('code 2: %s' % struct.unpack('4s', f.read(4)))
print('code 3: %s' % struct.unpack('4s', f.read(4)))
print('code 4: %s' % struct.unpack('4s', f.read(4)))
print('code 5: %s' % struct.unpack('4s', f.read(4)))

# dumpConstants
print('sizek: %d' % struct.unpack('i', f.read(4)))
# a = 18
print('String类型: %x' % struct.unpack('B', f.read(1)))
printString()
print('Integer类型: %x' % struct.unpack('B', f.read(1)))
print('Integer值: %d' % struct.unpack('q', f.read(8)))
# print('hello world')
print('String类型: %x' % struct.unpack('B', f.read(1)))
printString()
print('String类型: %x' % struct.unpack('B', f.read(1)))
printString()

# dumpUpvalues（外部局部变量）
print('sizeupvalues: %d' % struct.unpack('i', f.read(4)))
print('instack: %x' % struct.unpack('B', f.read(1)))
print('idx: %x' % struct.unpack('B', f.read(1)))

# dumpProtos（子函数）
print('sizep: %d' % struct.unpack('i', f.read(4)))

# dumpDebug
print('sizelineinfo: %d' % struct.unpack('i', f.read(4)))
print('line 1: %d' % struct.unpack('i', f.read(4)))
print('line 2: %d' % struct.unpack('i', f.read(4)))
print('line 3: %d' % struct.unpack('i', f.read(4)))
print('line 4: %d' % struct.unpack('i', f.read(4)))
print('line 5: %d' % struct.unpack('i', f.read(4)))
print('sizelocvars: %d' % struct.unpack('i', f.read(4)))
print('sizeupvalues: %d' % struct.unpack('i', f.read(4)))
printString()

f.close()

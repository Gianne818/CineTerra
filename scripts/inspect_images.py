import struct

def get_image_info(data):
    # PNG
    if data.startswith(b'\x89PNG\r\n\x1a\n'):
        w, h = struct.unpack('>LL', data[16:24])
        return 'PNG', w, h
    # JPEG
    elif data.startswith(b'\xff\xd8'):
        idx = 2
        while idx < len(data):
            marker, length = struct.unpack('>2sH', data[idx:idx+4])
            idx += 2
            if marker in (b'\xff\xc0', b'\xff\xc2'): # SOF0, SOF2
                h, w = struct.unpack('>HH', data[idx+1:idx+5])
                return 'JPEG', w, h
            idx += length
        return 'JPEG', None, None
    # WEBP
    elif data[:4] == b'RIFF' and data[8:12] == b'WEBP':
        chunk_type = data[12:16]
        if chunk_type == b'VP8 ':
            w, h = struct.unpack('<HH', data[26:30])
            return 'WEBP-VP8', w & 0x3fff, h & 0x3fff
        elif chunk_type == b'VP8L':
            b0, b1, b2, b3 = data[21:25]
            w = 1 + (((b1 & 0x3F) << 8) | b0)
            h = 1 + (((b3 & 0xF) << 10) | (b2 << 2) | ((b1 & 0xC0) >> 6))
            return 'WEBP-VP8L', w, h
        elif chunk_type == b'VP8X':
            w = 1 + int.from_bytes(data[24:27], byteorder='little')
            h = 1 + int.from_bytes(data[27:30], byteorder='little')
            return 'WEBP-VP8X', w, h
        return 'WEBP', None, None
    return 'UNKNOWN', None, None

if __name__ == '__main__':
    import sys
    for path in sys.argv[1:]:
        with open(path, 'rb') as f:
            d = f.read()
        fmt, w, h = get_image_info(d)
        print(f"{path}: {fmt} {w}x{h} ({len(d)} bytes)")

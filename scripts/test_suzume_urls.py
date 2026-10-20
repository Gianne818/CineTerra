import urllib.request
import ssl

ctx = ssl.create_default_context()
ctx.check_hostname = False
ctx.verify_mode = ssl.CERT_NONE

urls = [
    ('bungo_still_1', 'https://static.wikia.nocookie.net/makotoshinkai/images/4/4d/Suzume_no_Tojimari.jpeg/revision/latest'),
    ('bungo_still_2', 'https://upload.wikimedia.org/wikipedia/commons/c/cc/Suzume_Door_at_Bungo-Mori_Roundhouse.jpg'),
    ('bungo_still_3', 'https://static.wikia.nocookie.net/makotoshinkai/images/b/b8/Ever-After.jpg/revision/latest'),
    ('bungo_real_1', 'https://upload.wikimedia.org/wikipedia/commons/4/45/Bungo-Mori_Roundhouse_20180331-1.jpg'),
    ('hijiribashi_still_1', 'https://static.wikia.nocookie.net/makotoshinkai/images/8/8c/Hijiribashi_Bridge_Suzume.png/revision/latest?cb=20230329015251'),
    ('hijiribashi_real_1', 'https://static.wikia.nocookie.net/makotoshinkai/images/f/f2/Hijiribashi_Bridge.jpg/revision/latest?cb=20230329015154'),
    ('hijiribashi_real_2', 'https://upload.wikimedia.org/wikipedia/commons/a/a1/Hijiribashi_-01.jpg')
]

for name, url in urls:
    headers = {
        'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36'
    }
    if 'fandom.com' in url or 'wikia.nocookie.net' in url:
        headers['Referer'] = 'https://www.fandom.com/'
    elif 'wikimedia.org' in url or 'wikipedia.org' in url:
        headers['Referer'] = 'https://commons.wikimedia.org/'

    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req, context=ctx, timeout=10) as r:
            info = r.info()
            length = info.get('Content-Length', 'unknown')
            ctype = info.get_content_type()
            print(f'{name}: OK ({r.status}) - {ctype} - {length} bytes - URL: {url}')
    except Exception as e:
        print(f'{name}: FAILED - {e}')

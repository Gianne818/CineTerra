import urllib.request
import os

headers = {'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; Win64; x64)'}

imgs = [
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_01-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_02-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_04.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_05-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_06-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_07-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_08-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_09-1.jpg',
    'https://ftrc.blog/wp-content/uploads/2025/04/Inception-Bridge-aka-Pont-de-Bir-Hakeim-in-Paris_FTRC_Meme.jpeg'
]

for url in imgs:
    fn = os.path.basename(url.split('?')[0])
    dest = f"temp_trailers/{fn}"
    try:
        req = urllib.request.Request(url, headers=headers)
        with urllib.request.urlopen(req, timeout=10) as resp:
            data = resp.read()
            with open(dest, 'wb') as f:
                f.write(data)
            print(f"Downloaded {fn} ({len(data)} bytes)")
    except Exception as e:
        print(f"Failed {url}: {e}")

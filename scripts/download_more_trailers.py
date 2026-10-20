import urllib.request
import os

os.makedirs('temp_trailers', exist_ok=True)
headers = {'User-Agent': 'Mozilla/5.0', 'Referer': 'https://www.fandom.com/'}

urls = {
    f'Trailer{i:02d}': f'https://static.wikia.nocookie.net/inception/images/0/00/Trailer{i:02d}.jpg/revision/latest'
    for i in range(21, 39)
}
# Also get exact wiki imageinfo URLs
import json
titles = "|".join([f"File:Trailer{i:02d}.jpg" for i in range(21, 39)])
url = f"https://inception.fandom.com/api.php?action=query&titles={urllib.parse.quote(titles)}&prop=imageinfo&iiprop=url&format=json"
req = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0'})
with urllib.request.urlopen(req) as resp:
    data = json.loads(resp.read().decode('utf-8'))
    for k, v in data['query']['pages'].items():
        title = v.get('title')
        info = v.get('imageinfo', [{}])[0]
        name = title.replace('File:', '').replace('.jpg', '')
        u = info.get('url')
        if u:
            dest = f"temp_trailers/{name}.jpg"
            try:
                r = urllib.request.Request(u, headers=headers)
                with urllib.request.urlopen(r) as resp2:
                    with open(dest, 'wb') as f:
                        f.write(resp2.read())
                print(f"Downloaded {dest}")
            except Exception as e:
                print(f"Failed {name}: {e}")

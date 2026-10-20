import urllib.request
import urllib.parse
import json

headers = {'User-Agent': 'Mozilla/5.0'}

# Query imageusage for each trailer
for i in range(2, 39):
    fn = f"File:Trailer{i:02d}.jpg"
    url = f"https://inception.fandom.com/api.php?action=query&list=imageusage&iutitle={urllib.parse.quote(fn)}&format=json"
    req = urllib.request.Request(url, headers=headers)
    try:
        with urllib.request.urlopen(req) as resp:
            d = json.loads(resp.read().decode('utf-8'))
            usages = d.get('query', {}).get('imageusage', [])
            if usages:
                titles = [u.get('title') for u in usages]
                print(f"{fn} -> {titles}")
    except Exception as e:
        pass

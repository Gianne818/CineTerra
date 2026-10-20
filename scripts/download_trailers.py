import urllib.request
import os

os.makedirs('temp_trailers', exist_ok=True)
headers = {'User-Agent': 'Mozilla/5.0', 'Referer': 'https://www.fandom.com/'}

urls = {
    'Trailer11': 'https://static.wikia.nocookie.net/inception/images/b/b5/Trailer11.jpg/revision/latest?cb=20100715204619',
    'Trailer12': 'https://static.wikia.nocookie.net/inception/images/0/06/Trailer12.jpg/revision/latest?cb=20100715204620',
    'Trailer13': 'https://static.wikia.nocookie.net/inception/images/5/58/Trailer13.jpg/revision/latest?cb=20100715204620',
    'Trailer14': 'https://static.wikia.nocookie.net/inception/images/b/bb/Trailer14.jpg/revision/latest?cb=20100715204620',
    'Trailer15': 'https://static.wikia.nocookie.net/inception/images/3/3b/Trailer15.jpg/revision/latest?cb=20100715204621',
    'Trailer16': 'https://static.wikia.nocookie.net/inception/images/6/6f/Trailer16.jpg/revision/latest?cb=20100715204621',
    'Trailer17': 'https://static.wikia.nocookie.net/inception/images/4/4e/Trailer17.jpg/revision/latest?cb=20100715204622',
    'Trailer18': 'https://static.wikia.nocookie.net/inception/images/c/c6/Trailer18.jpg/revision/latest?cb=20100715204622',
    'Trailer19': 'https://static.wikia.nocookie.net/inception/images/1/1c/Trailer19.jpg/revision/latest?cb=20100715204623',
    'Trailer20': 'https://static.wikia.nocookie.net/inception/images/f/fd/Trailer20.jpg/revision/latest?cb=20100715204623',
}

for name, url in urls.items():
    dest = f"temp_trailers/{name}.jpg"
    req = urllib.request.Request(url, headers=headers)
    with urllib.request.urlopen(req) as resp:
        data = resp.read()
        with open(dest, 'wb') as f:
            f.write(data)
    print(f"Downloaded {dest}: {len(data)} bytes")

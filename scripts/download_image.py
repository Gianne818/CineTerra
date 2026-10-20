#!/usr/bin/env python3
import sys
import os
import argparse
import urllib.request
import urllib.parse
import ssl

DEFAULT_UA = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"

def download(url, dest_path, referer=None):
    os.makedirs(os.path.dirname(os.path.abspath(dest_path)), exist_ok=True)
    
    headers = {
        "User-Agent": DEFAULT_UA,
        "Accept": "image/avif,image/webp,image/apng,image/svg+xml,image/*,*/*;q=0.8"
    }
    if "wikimedia.org" in url or "wikipedia.org" in url:
        headers["User-Agent"] = "CineTerra/1.0 (contact@cineterra.org) Python/3"
        headers["Referer"] = "https://commons.wikimedia.org/"
    elif "fandom.com" in url or "wikia.nocookie.net" in url:
        headers["Referer"] = "https://www.fandom.com/"

    req = urllib.request.Request(url, headers=headers)
    ctx = ssl.create_default_context()
    ctx.check_hostname = False
    ctx.verify_mode = ssl.CERT_NONE

    print(f"Downloading: {url} -> {dest_path}")
    try:
        with urllib.request.urlopen(req, context=ctx, timeout=30) as resp:
            data = resp.read()
            if len(data) < 1000 and b"<html" in data.lower():
                print(f"Error: Downloaded HTML error page instead of image ({len(data)} bytes).")
                return False
            with open(dest_path, "wb") as f:
                f.write(data)
            
            # Normalize to true format matching extension
            if dest_path.lower().endswith(('.jpg', '.jpeg')):
                try:
                    from PIL import Image
                    with Image.open(dest_path) as im:
                        if im.format != 'JPEG':
                            im.convert('RGB').save(dest_path, 'JPEG', quality=95)
                            print(f"Normalized format from {im.format} to JPEG.")
                except Exception:
                    pass
            elif dest_path.lower().endswith('.png'):
                try:
                    from PIL import Image
                    with Image.open(dest_path) as im:
                        if im.format != 'PNG':
                            im.save(dest_path, 'PNG')
                            print(f"Normalized format from {im.format} to PNG.")
                except Exception:
                    pass

            file_size = os.path.getsize(dest_path)
            print(f"Success: Saved {dest_path} ({file_size} bytes).")
            return True
    except Exception as e:
        print(f"Failed to download {url}: {e}")
        return False

if __name__ == "__main__":
    parser = argparse.ArgumentParser()
    parser.add_argument("--url", required=True, help="Image URL to download")
    parser.add_argument("--dest", required=True, help="Destination local path")
    parser.add_argument("--referer", help="Optional HTTP Referer header")
    args = parser.parse_args()

    success = download(args.url, args.dest, args.referer)
    sys.exit(0 if success else 1)

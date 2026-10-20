#!/usr/bin/env python3
import sys
import os
import json
import time

LOCATIONS_FILE = "src/CineTerra/wwwroot/data/locations.json"
PRODUCTIONS_FILE = "src/CineTerra/wwwroot/data/productions.json"
HOME_FILE = "src/CineTerra/Pages/Home.razor"
LOCK_FILE = "src/CineTerra/wwwroot/data/data.lock"

def acquire_lock(timeout=30):
    start = time.time()
    while time.time() - start < timeout:
        try:
            fd = os.open(LOCK_FILE, os.O_CREAT | os.O_EXCL | os.O_RDWR)
            os.close(fd)
            return True
        except FileExistsError:
            time.sleep(0.1)
    return False

def release_lock():
    try:
        if os.path.exists(LOCK_FILE):
            os.remove(LOCK_FILE)
    except Exception:
        pass

def update_locations(location_updates):
    """
    location_updates: dict of location_id -> {
        'imageUrl': ...,
        'scenes': [ { 'productionId': ..., 'stillImageUrl': ..., 'realImageUrl': ... } ]
    }
    """
    if not acquire_lock():
        print("Failed to acquire lock for locations update")
        return False

    try:
        with open(LOCATIONS_FILE, 'r', encoding='utf-8') as f:
            locations = json.load(f)

        for loc in locations:
            loc_id = loc.get('id')
            if loc_id in location_updates:
                upd = location_updates[loc_id]
                if 'imageUrl' in upd and upd['imageUrl']:
                    loc['imageUrl'] = upd['imageUrl']
                if 'scenes' in upd:
                    for i, scene in enumerate(loc.get('scenes', [])):
                        if i < len(upd['scenes']):
                            s_upd = upd['scenes'][i]
                            if 'stillImageUrl' in s_upd and s_upd['stillImageUrl']:
                                scene['stillImageUrl'] = s_upd['stillImageUrl']
                            if 'realImageUrl' in s_upd and s_upd['realImageUrl']:
                                scene['realImageUrl'] = s_upd['realImageUrl']

        with open(LOCATIONS_FILE, 'w', encoding='utf-8') as f:
            json.dump(locations, f, indent=2, ensure_ascii=False)
            f.write('\n')

        print(f"Successfully updated locations.json for {list(location_updates.keys())}")
        return True
    finally:
        release_lock()

def update_productions(prod_id, poster_url=None, backdrop_url=None):
    if not acquire_lock():
        print("Failed to acquire lock for productions update")
        return False

    try:
        with open(PRODUCTIONS_FILE, 'r', encoding='utf-8') as f:
            prods = json.load(f)

        for p in prods:
            if p.get('id') == prod_id:
                if poster_url:
                    p['posterUrl'] = poster_url
                if backdrop_url:
                    p['backdropUrl'] = backdrop_url

        with open(PRODUCTIONS_FILE, 'w', encoding='utf-8') as f:
            json.dump(prods, f, indent=2, ensure_ascii=False)
            f.write('\n')

        print(f"Successfully updated productions.json for {prod_id}")
        return True
    finally:
        release_lock()

def update_home_highlight(location_id, new_image_url):
    if not acquire_lock():
        print("Failed to acquire lock for Home.razor update")
        return False

    try:
        with open(HOME_FILE, 'r', encoding='utf-8') as f:
            content = f.read()

        # Find ScenicChapter with LocationId: "location_id" and update its ImageUrl: "..."
        import re
        pattern = rf'(LocationId:\s*"{re.escape(location_id)}"(?:(?!\bnew ScenicChapter\b).)*?ImageUrl:\s*")[^"]+(")'
        new_content, count = re.subn(pattern, rf'\g<1>{new_image_url}\g<2>', content, flags=re.DOTALL)
        if count > 0:
            with open(HOME_FILE, 'w', encoding='utf-8') as f:
                f.write(new_content)
            print(f"Successfully updated Home.razor highlight for {location_id} -> {new_image_url}")
            return True
        else:
            print(f"No match found in Home.razor for {location_id}")
            return False
    finally:
        release_lock()

if __name__ == '__main__':
    if len(sys.argv) > 1 and sys.argv[1] == '--test':
        print("update_film_asset helper ready.")

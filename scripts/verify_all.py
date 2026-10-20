import json
import os
import re

with open("src/CineTerra/wwwroot/data/locations.json", "r", encoding="utf-8") as f:
    locations = json.load(f)

print(f"Total locations: {len(locations)}")
missing_files = []

for loc in locations:
    print(f"\n--- Location: {loc['id']} ({loc.get('name')}) ---")
    hero = loc.get('imageUrl')
    print(f"  Hero Image: {hero}")
    if hero and not hero.startswith("http"):
        hero_disk = os.path.join("src/CineTerra/wwwroot", hero.replace("/", os.sep))
        if not os.path.exists(hero_disk):
            missing_files.append(hero_disk)
            print(f"    [MISSING] {hero_disk}")
        else:
            print(f"    [OK] {hero_disk} ({os.path.getsize(hero_disk):,} bytes)")

    for sc in loc.get("scenes", []):
        still = sc.get("stillImageUrl")
        real = sc.get("realImageUrl")
        print(f"  Scene: \"{sc.get('title')}\"")
        print(f"    Still: {still}")
        print(f"    Real:  {real}")
        for path in [still, real]:
            if path and not path.startswith("http"):
                disk_path = os.path.join("src/CineTerra/wwwroot", path.replace("/", os.sep))
                if not os.path.exists(disk_path):
                    missing_files.append(disk_path)
                    print(f"      [MISSING] {disk_path}")
                else:
                    print(f"      [OK] {disk_path} ({os.path.getsize(disk_path):,} bytes)")

print("\n===============================")
print(f"Any missing files in locations.json? {len(missing_files)}")

# Check Home.razor image URLs
with open("src/CineTerra/Pages/Home.razor", "r", encoding="utf-8") as f:
    home_content = f.read()

home_images = re.findall(r'ImageUrl:\s*"([^"]+)"', home_content)
print(f"\nHome.razor ImageUrls: {len(home_images)}")
for img in home_images:
    print(f"  Home Image: {img}")
    if not img.startswith("http"):
        disk_path = os.path.join("src/CineTerra/wwwroot", img.replace("/", os.sep))
        if not os.path.exists(disk_path):
            missing_files.append(disk_path)
            print(f"    [MISSING] {disk_path}")
        else:
            print(f"    [OK] {disk_path} ({os.path.getsize(disk_path):,} bytes)")

print("\n===============================")
print(f"Total missing files across project: {len(missing_files)}")
if missing_files:
    for m in missing_files:
        print(f"  Missing: {m}")
else:
    print("ALL image files referenced in locations.json and Home.razor exist on disk and are valid!")

import os
import time
from playwright.sync_api import sync_playwright

# Cấu hình
TARGET_URL = "https://fuoverflow.com/threads/jpd113-sp26-re.6329/"
DOWNLOAD_DIR = "downloads/jpd113"

def main():
    if not os.path.exists(DOWNLOAD_DIR):
        os.makedirs(DOWNLOAD_DIR)

    with sync_playwright() as p:
        # Sử dụng trình duyệt Chromium
        browser = p.chromium.launch(headless=False) # Chạy có giao diện để đăng nhập
        context = browser.new_context()
        page = context.new_page()

        print(f"\n[*] Đang mở trang: {TARGET_URL}")
        page.goto(TARGET_URL)

        print("\n[!] QUAN TRỌNG: Vui lòng đăng nhập vào FuOverflow trên cửa sổ trình duyệt vừa mở.")
        print("[!] Sau khi đăng nhập xong, hãy quay lại đây và nhấn phím ENTER để tiếp tục...")
        input()

        # Quét các phần tử chứa ảnh đính kèm
        print("[*] Đang quét các tệp đính kèm...")
        
        # XenForo lưu link ảnh trong các thẻ <a> có class 'js-lbImage'
        attachment_links = page.query_selector_all("a.js-lbImage")
        
        if not attachment_links:
            print("[!] Không tìm thấy ảnh đính kèm nào. Có thể bạn chưa đăng nhập hoặc selector đã thay đổi.")
            browser.close()
            return

        print(f"[*] Tìm thấy {len(attachment_links)} ảnh. Bắt đầu tải về...")

        for i, link in enumerate(attachment_links):
            try:
                href = link.get_attribute("href")
                if not href:
                    continue
                
                # Trích xuất ID từ href (ví dụ: q1-jpg.260014/ -> 260014)
                # Cấu trúc: /attachments/filename.ID/
                parts = href.strip("/").split(".")
                if len(parts) < 2:
                    continue
                
                attach_id = parts[-1]
                download_url = f"https://fuoverflow.com/attachments/{attach_id}/download"
                
                filename = f"Q{i+1}.jpg"
                filepath = os.path.join(DOWNLOAD_DIR, filename)

                print(f"  [>] Đang tải {filename} từ ID: {attach_id}...")
                
                # Tải file bằng request của page để tận dụng session/cookies
                response = page.request.get(download_url)
                if response.status == 200:
                    with open(filepath, "wb") as f:
                        f.write(response.body())
                    print(f"  [+] Thành công: {filename}")
                else:
                    print(f"  [-] Thất bại {filename}: HTTP {response.status}")
                
                time.sleep(1.5) # Tránh bị rate limit
                
            except Exception as e:
                print(f"  [-] Lỗi nghiêm trọng tại ảnh {i+1}: {e}")

        print(f"\n[OK] Hoàn tất! Ảnh đã được lưu tại thư mục: {os.path.abspath(DOWNLOAD_DIR)}")
        browser.close()

if __name__ == "__main__":
    main()

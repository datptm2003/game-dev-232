# Luật chơi: ĐẬP ZOMBIE

**Đập zombie** là trò chơi được mô phỏng lại dựa trên một trò chơi khác mang tên Đập chuột. Nhiệm vụ chính của người chơi rất đơn giản: đập trúng nhiều chú chuột nhất có thể ngay khi chúng chui lên từ hang và trước khi chúng núp xuống trở lại. Theo đó, nhóm quyết định thiết kế một tựa game tương tự, với một số sự thay đổi về hình ảnh và cơ chế như sau:

## Thành phần chính

- Một bản đồ hang có kích thước `a x b`, tức `a` hang theo chiều dọc và `b` hang theo chiều ngang.
- Các chú "zombie" đang trốn bên dưới hệ thống hang.
- Một món vũ khí dùng để đập zombie.

## Các loại zombie

- **Zombie thường:** loại zombie yếu nhất, đập 1 lần để chết. Nếu zombie chết, người chơi hưởng 100 điểm. Nếu zombie không chết (miss), người chơi mất 10 điểm.
- **Zombie nổ:** loại zombie dị năng, đập 2 lần để chết. Nếu zombie chết, người chơi hưởng 250 điểm nhưng không thể đập tiếp trong vòng 2 giây. Nếu zombie không chết, người chơi mất 20 điểm.
- **Zombie chúa:** loại zombie mạnh nhất, đập 3 lần để chết. Nếu zombie chết, người chơi hưởng 350 điểm. Nếu zombie không chết, người chơi mất 70 điểm.
- **Con người (!):** không được đập dù chỉ 1 lần. Nếu bị đập, con người sẽ chết và người chơi mất 120 điểm.  

## Hiệu ứng đặc biệt

Đôi khi, một số vật phẩm mang đến cho người chơi các hiệu ứng đặc biệt (lợi hoặc hại) cũng xuất hiện từ hang.
- **Vật phẩm 1 (lợi):** Nhân đôi số điểm được hưởng đối với mỗi zombie bị chết trong vòng 5 giây kế tiếp. Số điểm bị trừ nếu giết hụt được giữ nguyên.
- **Vật phẩm 2 (lợi):** Tiêu diệt toàn bộ zombie hiện có trên bản đồ và hưởng toàn bộ số điểm như khi giết chúng lúc bình thường.
- **Vật phẩm 3 (hại):** Chia đôi số điểm được hưởng đối với mỗi zombie bị chết trong vòng 5 giây kế tiếp. Số điểm bị trừ nếu giết hụt được nhân đôi.
- **Vật phẩm 4 (hại):** Người chơi ngay lập tức mất 600 điểm.

## Trang bị

Trước mỗi ván game, người chơi được quyền lựa chọn giữa các loại trang bị dùng để đập zombie, mỗi loại có một năng lực riêng biệt.
- **Búa gỗ:** Trang bị mặc định, không có hiệu ứng.
- **Búa thép:** Như búa gỗ, nhưng có thể loại bỏ hiệu ứng bất lợi của zombie nổ nếu đập bằng chuột phải.
- **Búa sấm sét:** Như búa thép, nhưng có thể giảm 50% hiệu ứng xấu từ bùa hại.
    - Giảm thời gian hiệu ứng của vật phẩm 3 còn 2.5 giây.
    - Giảm số điểm bị mất của vật phẩm 4 còn 300 điểm.

# Cấp độ

- **Dễ:** Thời gian zombie xuất hiện dài, được sử dụng bất kỳ loại trang bị nào.
- **Trung bình:** Thời gian zombie xuất hiện ngắn, được sử dụng bất kỳ loại trang bị nào.
- **Khó:** Thời gian zombie xuất hiện rất ngắn, chỉ được dùng búa gỗ.
using BTGioTanHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using OfficeOpenXml;

namespace BTGioTanHoc.Controllers
{
    public class ExcelController : Controller
    {
        private readonly Entities _context;
        // GET: Excel
        public ExcelController()
        {
            _context = new Entities();
        }
        public ActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return Json(new { success = false, message = "Vui lòng chọn một file Excel." });
            }

            try
            {
                using (var package = new ExcelPackage(file.InputStream))
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault(); // Lấy sheet đầu tiên

                    if (worksheet == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy sheet nào trong file Excel." });
                    }

                    int rowCount = worksheet.Dimension.Rows;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        if (worksheet.Cells[row, 2].Text.Trim() == "") continue; // Bỏ qua hàng trống

                        var maLHP = worksheet.Cells[row, 2]?.Text?.Trim() ?? "";
                        var tenMonHoc = worksheet.Cells[row, 3]?.Text?.Trim() ?? "";
                        var lopHoc = worksheet.Cells[row, 4]?.Text?.Trim() ?? "";
                        var tietHoc = worksheet.Cells[row, 8]?.Text?.Trim() ?? "";
                        var ngayHoc = worksheet.Cells[row, 9]?.Text?.Trim() ?? "";
                        var siSo = worksheet.Cells[row, 11]?.Text?.Trim() ?? "";
                        var phongHoc = worksheet.Cells[row, 12]?.Text?.Trim() ?? "";
                        var dayNha = worksheet.Cells[row, 14]?.Text?.Trim() ?? "";
                        var giangVien = worksheet.Cells[row, 15]?.Text?.Trim() ?? "";

                        if (string.IsNullOrEmpty(maLHP) || string.IsNullOrEmpty(tenMonHoc))
                        {
                            continue;
                        }

                        var tietValues = tietHoc.Split('→');
                        int tietBatDau = int.Parse(tietValues[0].Trim());
                        int tietKetThuc = int.Parse(tietValues[1].Trim());

                        // Thêm môn học
                        var monHoc = _context.MonHoc.FirstOrDefault(m => m.TenMH == tenMonHoc);
                        if (monHoc == null)
                        {
                            monHoc = new MonHoc { TenMH = tenMonHoc };
                            _context.MonHoc.Add(monHoc);
                            _context.SaveChanges();
                        }

                        // Thêm giảng viên
                        var giangVienEntity = _context.GiangVien.FirstOrDefault(g => g.HoTenGiangVien == giangVien);
                        if (giangVienEntity == null)
                        {
                            giangVienEntity = new GiangVien { HoTenGiangVien = giangVien };
                            _context.GiangVien.Add(giangVienEntity);
                            _context.SaveChanges();
                        }

                        // Thêm phòng học
                        var phongHocEntity = _context.PhongHoc.FirstOrDefault(p => p.TenPhong == phongHoc);
                        if (phongHocEntity == null)
                        {
                            phongHocEntity = new PhongHoc { TenPhong = phongHoc, DayNha = dayNha };
                            _context.PhongHoc.Add(phongHocEntity);
                            _context.SaveChanges();
                        }
                        // Thêm lớp danh nghĩa
                        var lopDanhNghia = _context.LopDanhNghia.FirstOrDefault(h => h.TenLopDN == lopHoc);
                        if (lopDanhNghia == null)
                        {
                            lopDanhNghia = new LopDanhNghia { TenLopDN = lopHoc };
                            _context.LopDanhNghia.Add(lopDanhNghia);
                            _context.SaveChanges();
                        }
                        // Kiểm tra xem MaLHP đã tồn tại chưa
                        var existingLopHocPhan = _context.LopHocPhan.FirstOrDefault(l => l.MaLHP == maLHP);
                        LopHocPhan lopHocPhanEntity;

                        if (existingLopHocPhan == null)
                        {
                            // Nếu chưa tồn tại, thêm mới
                            var lopHocPhan = new LopHocPhan
                            {
                                MaLHP = maLHP,
                                MaLopDN = lopDanhNghia.MaLopDN,
                                SiSo = int.Parse(siSo),
                                MaGiangVien = giangVienEntity.MaGiangVien,
                                MaMH = monHoc.MaMH
                            };
                            _context.LopHocPhan.Add(lopHocPhan);
                            _context.SaveChanges();

                            lopHocPhanEntity = lopHocPhan; // Gán biến để sử dụng sau
                        }
                        else
                        {
                            // Nếu đã tồn tại, có thể cập nhật thông tin (nếu cần)
                            existingLopHocPhan.SiSo = int.Parse(siSo);
                            existingLopHocPhan.MaGiangVien = giangVienEntity.MaGiangVien;
                            _context.SaveChanges();

                            lopHocPhanEntity = existingLopHocPhan; // Gán biến để sử dụng sau
                        }

                        // Sau khi có chắc chắn MaLHP, tạo lịch học
                        var lichHoc = new LichHoc
                        {
                            MaLHP = lopHocPhanEntity.MaLHP, // Sử dụng biến đã xác định
                            MaPhong = phongHocEntity.MaPhong,
                            NgayHoc = DateTime.Parse(ngayHoc),
                            TietBatDau = tietBatDau,
                            TietKetThuc = tietKetThuc
                        };
                        _context.LichHoc.Add(lichHoc);
                        _context.SaveChanges();

                    }

                    _context.SaveChanges();
                }

                return Json(new { success = true, message = "Dữ liệu từ file Excel đã được lưu vào CSDL thành công." });
            }
            catch (DbEntityValidationException ex)
            {
                var errors = ex.EntityValidationErrors
                    .SelectMany(e => e.ValidationErrors)
                    .Select(e => $"{e.PropertyName}: {e.ErrorMessage}")
                    .ToList();

                return Json(new { success = false, message = "Validation Error", details = errors });
            }
        }
    }
}
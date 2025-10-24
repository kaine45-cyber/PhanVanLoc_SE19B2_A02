# ==============================
# Auto Rename C# Project Script
# From: PhungDangTruong
# To:   PhanVanLoc
# ==============================

param(
    [string]$OldName = "PhungDangTruong",
    [string]$NewName = "PhanVanLoc"
)

Write-Host "=== Renaming from '$OldName' to '$NewName' ===" -ForegroundColor Cyan

# Đổi tên tất cả thư mục chứa OldName
Get-ChildItem -Recurse -Directory | Where-Object { $_.Name -match $OldName } | ForEach-Object {
    $newFolderName = $_.FullName -replace $OldName, $NewName
    Write-Host "Renaming folder: $($_.FullName) -> $newFolderName"
    Rename-Item $_.FullName $newFolderName
}

# Đổi tên tất cả file có OldName trong tên
Get-ChildItem -Recurse -File | Where-Object { $_.Name -match $OldName } | ForEach-Object {
    $newFileName = $_.FullName -replace $OldName, $NewName
    Write-Host "Renaming file: $($_.FullName) -> $newFileName"
    Rename-Item $_.FullName $newFileName
}

# Thay text bên trong tất cả file code (.sln, .csproj, .cs, .config, .xaml, ...)
Get-ChildItem -Recurse -File | Where-Object {
    $_.Extension -match '(\.sln|\.csproj|\.cs|\.xaml|\.config|\.resx|\.xml)'
} | ForEach-Object {
    (Get-Content $_.FullName -Raw) -replace $OldName, $NewName | Set-Content $_.FullName -Encoding UTF8
    Write-Host "Updated contents: $($_.FullName)"
}

Write-Host "`n=== ✅ Rename complete! ===" -ForegroundColor Green

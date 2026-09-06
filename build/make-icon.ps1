Add-Type -AssemblyName System.Drawing

function New-IconBitmap([int]$size) {
    $bmp = New-Object System.Drawing.Bitmap($size, $size)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
    $g.Clear([System.Drawing.Color]::Transparent)

    # Capsule body
    $pad = [Math]::Max(1, [int]($size * 0.06))
    $h = [int]($size * 0.46)
    $y = ($size - $h) / 2
    $rect = New-Object System.Drawing.Rectangle($pad, $y, $size - 2*$pad, $h)
    $radius = $h / 2

    $path = New-Object System.Drawing.Drawing2D.GraphicsPath
    $d = $radius * 2
    $path.AddArc($rect.X, $rect.Y, $d, $d, 180, 90)
    $path.AddArc($rect.Right - $d, $rect.Y, $d, $d, 270, 90)
    $path.AddArc($rect.Right - $d, $rect.Bottom - $d, $d, $d, 0, 90)
    $path.AddArc($rect.X, $rect.Bottom - $d, $d, $d, 90, 90)
    $path.CloseFigure()

    # Gradient fill (purple -> blue -> teal)
    $c1 = [System.Drawing.Color]::FromArgb(255, 99, 102, 241)   # indigo
    $c2 = [System.Drawing.Color]::FromArgb(255, 34, 211, 238)   # cyan
    $brush = New-Object System.Drawing.Drawing2D.LinearGradientBrush($rect, $c1, $c2, 30.0)
    $g.FillPath($brush, $path)

    # Play triangle (white, semi-transparent)
    $ts = [int]($h * 0.30)
    $tx = [int]($size * 0.545)
    $ty = $y + ($h - $ts) / 2
    $tri = New-Object System.Drawing.Point[3]
    $tri[0] = New-Object System.Drawing.Point($tx, $ty)
    $tri[1] = New-Object System.Drawing.Point($tx, $ty + $ts)
    $tri[2] = New-Object System.Drawing.Point($tx + [int]($ts * 0.85), $ty + $ts/2)
    $white = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(235, 255, 255, 255))
    $g.FillPolygon($white, $tri)

    $g.Dispose()
    return $bmp
}

$outDir = 'E:\MyFiles\Program\WinIslands\src\WinIslands\Assets'
New-Item -ItemType Directory -Force -Path $outDir | Out-Null
$sizes = 16,24,32,48,64,128,256
$icon = New-Object System.Drawing.Icon
$ms = New-Object System.IO.MemoryStream
$bw = New-Object System.IO.BinaryWriter($ms)
$bw.Write([uint16]0)
$bw.Write([uint16]1)
$bw.Write([uint16]$sizes.Count)
$offset = 6 + 16 * $sizes.Count
$datas = @()
foreach ($s in $sizes) {
    $bmp = New-IconBitmap $s
    $pms = New-Object System.IO.MemoryStream
    $bmp.Save($pms, [System.Drawing.Imaging.ImageFormat]::Png)
    $data = $pms.ToArray()
    $bw.Write([byte]($(if ($s -ge 256) {0} else {$s})))
    $bw.Write([byte]($(if ($s -ge 256) {0} else {$s})))
    $bw.Write([byte]0)
    $bw.Write([byte]0)
    $bw.Write([uint16]1)
    $bw.Write([uint16]32)
    $bw.Write([uint32]$data.Length)
    $bw.Write([uint32]$offset)
    $offset += $data.Length
    $datas += ,$data
    $bmp.Dispose(); $pms.Dispose()
}
foreach ($d in $datas) { $bw.Write($d) }
$bw.Flush()
[System.IO.File]::WriteAllBytes("$outDir\winislands.ico", $ms.ToArray())
$bw.Dispose(); $ms.Dispose()
Write-Output "icon written: $((Get-Item "$outDir\winislands.ico").Length) bytes"

# 1. Set the project root to one level up from the /run folder
$ProjectRoot = (Get-Item $PSScriptRoot).Parent.FullName

# 2. Output file path
$OutputFile = Join-Path $ProjectRoot "full_project_code.txt"

# 3. Define folders to exclude (bin, obj, .vs, etc.)
$ExcludeFolders = @('.git', '.vs', '.idea', 'bin', 'obj', 'run', 'packages', 'TestResults')

# 4. Define file extensions to collect for a C# / .NET project
$Extensions = @('.cs', '.json', '.csproj', '.slnx', '.sln', '.txt', '.md')

Write-Host "Scanning cpu6502_proteus in: $ProjectRoot" -ForegroundColor Cyan

# 5. Find all files recursively
$allFiles = Get-ChildItem -Path $ProjectRoot -Recurse -File | Where-Object {
    $itemPath = $_.FullName
    $shouldSkip = $false
    foreach ($dir in $ExcludeFolders) {
        if ($itemPath -like "*\$dir\*") { $shouldSkip = $true; break }
    }
    !$shouldSkip -and ($Extensions -contains $_.Extension)
}

$report = New-Object System.Text.StringBuilder
[void]$report.AppendLine("=== CPU6502_PROTEUS PROJECT STRUCTURE ===")

# 6. Build the project tree
$structureItems = Get-ChildItem -Path $ProjectRoot -Recurse | Where-Object {
    $fullName = $_.FullName
    $skip = $false
    foreach ($d in $ExcludeFolders) { if ($fullName -like "*\$d\*") { $skip = $true; break } }
    !$skip
}

foreach ($item in $structureItems) {
    $rel = $item.FullName.Replace($ProjectRoot, "")
    if ($rel -eq "") { continue }
    $depth = $rel.Split('\').Count - 1
    $indent = "  " * $depth
    [void]$report.AppendLine("$indent$($item.Name)$(if($item.PSIsContainer){'/'})")
}

[void]$report.AppendLine("`n=== SOURCE CODE DUMP ===")

# 7. Append content
foreach ($f in $allFiles) {
    $relativeName = $f.FullName.Replace($ProjectRoot, "")
    Write-Host "Processing: $relativeName" -ForegroundColor Gray
    
    [void]$report.AppendLine("`n" + ('=' * 80))
    [void]$report.AppendLine("FILE: $relativeName")
    [void]$report.AppendLine(('=' * 80) + "`n")
    
    try {
        $content = Get-Content $f.FullName -Raw -Encoding UTF8
        [void]$report.AppendLine($content)
    } catch {
        [void]$report.AppendLine("Error reading file content.")
    }
    [void]$report.AppendLine("`n### END OF FILE ###")
}

# 8. Save the final report
$report.ToString() | Out-File -FilePath $OutputFile -Encoding UTF8 -Force
Write-Host "`nSuccess! Code gathered in: $OutputFile" -ForegroundColor Green
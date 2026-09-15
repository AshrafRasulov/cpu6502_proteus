import os

# Корень проекта cpu6502_proteus (на уровень выше папки run)
current_dir = os.path.dirname(os.path.abspath(__file__))
project_root = os.path.abspath(os.path.join(current_dir, '..'))
output_file = os.path.join(project_root, "full_project_code.txt")

EXTENSIONS = {'.cs', '.json', '.csproj', '.slnx', '.sln', '.txt', '.md'}
IGNORE_DIRS = {'.git', '.vs', '.idea', 'bin', 'obj', 'run', 'packages', 'TestResults'}

def generate_dump():
    print(f"📦 Сборка кода cpu6502_proteus из: {project_root}")
    
    with open(output_file, 'w', encoding='utf-8') as outfile:
        outfile.write("=== PROJECT STRUCTURE ===\n")
        
        for root, dirs, files in os.walk(project_root):
            dirs[:] = [d for d in dirs if d not in IGNORE_DIRS]
            level = root.replace(project_root, '').count(os.sep)
            indent = ' ' * 4 * level
            outfile.write(f"{indent}{os.path.basename(root)}/\n")
            
            for f in files:
                if any(f.endswith(ext) for ext in EXTENSIONS):
                    outfile.write(f"{' ' * 4 * (level + 1)}{f}\n")

        outfile.write("\n=== FILE CONTENTS ===\n")
        for root, dirs, files in os.walk(project_root):
            dirs[:] = [d for d in dirs if d not in IGNORE_DIRS]
            for file in files:
                if any(file.endswith(ext) for ext in EXTENSIONS):
                    path = os.path.join(root, file)
                    rel_path = os.path.relpath(path, project_root)
                    outfile.write(f"\n{'='*80}\nFILE: {rel_path}\n{'='*80}\n\n")
                    try:
                        with open(path, 'r', encoding='utf-8', errors='replace') as f:
                            outfile.write(f.read())
                    except:
                        outfile.write("❌ Error reading file")
                    outfile.write(f"\n\n{'#'*20} END OF {file} {'#'*20}\n")

    print(f"✅ Файл готов: {output_file}")

if __name__ == "__main__":
    generate_dump()
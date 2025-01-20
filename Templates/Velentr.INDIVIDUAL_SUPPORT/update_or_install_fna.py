import os
import subprocess

# Step 1: Get the directory path of the current script
current_file_directory = os.path.dirname(os.path.realpath(__file__))

# Step 2: Set our install path
fna_install_path = os.path.join(current_file_directory, ".fna")

# Step 2: Call fna_updator.py with the install path we want
fna_updator_path = os.path.join(current_file_directory, ".build", "fna_updator.py")
command = ["python", fna_updator_path, fna_install_path]

result = subprocess.run(command, capture_output=True, text=True)
print(result.stdout)
if result.stderr:
    print(result.stderr)
    exit(1)
exit(0)

import os
import subprocess

dir_path = os.path.dirname(os.path.realpath(__file__))
executable_path = os.path.join(dir_path, "fna_updater", "FnaUpdater.exe")

args = executable_path + " install " + dir_path + " FNA"
print("Executing: " + args)
subprocess.call(args, shell=True)

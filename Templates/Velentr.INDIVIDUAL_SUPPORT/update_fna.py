import os
import subprocess

dir_path = os.path.dirname(os.path.realpath(__file__))
executable_path = os.path.join(dir_path, ".build", "fna_updater", "FnaUpdater.exe")

args = executable_path + " update " + dir_path + " .fna"
print("Executing: " + args)
subprocess.call(args, shell=True)

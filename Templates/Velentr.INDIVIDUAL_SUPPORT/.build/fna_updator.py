""" --------------------------------------------------------------------
Imports
-------------------------------------------------------------------- """
import platform
from importlib import util
import shutil
import sys
import os
import subprocess


class FnaUpdator:
    """Manages and updates the FNA game development framework.

    This class handles installing, updating, and setting up the environment for FNA.  It includes
    functionality for cloning the FNA repository, installing necessary libraries, and managing
    virtual environments.
    """

    FNA_REPO: str = "https://github.com/FNA-XNA/FNA"
    """str: The URL for the FNA repository."""
    
    FNA_LIBS_LINK: str = "https://fna.flibitijibibo.com/archive/fnalibs.tar.bz2"
    """str: The URL for the FNA libs archive."""
    
    def __init__(self, directory: str) -> None:
        """Initializes the FNAUpdater.
        
        Sets up the necessary directories and installs the virtualenv package.  It also sets up the virtual environment
        if one doesn't already exist.
        
        Args:
            directory (str): The base directory for FNA installation and related files.
        """
        self._base_directory = directory
        self._fna_repo_install_path = os.path.join(directory, "FNA")
        self._fna_libs_install_path = os.path.join(directory, "fnalibs")
        self._virtual_environment_path = os.path.join(directory, ".venv")
        
        self._manage_directory(directory=self._base_directory, delete_directory_if_exists=True, create_directory_if_not_exists=True)
        self._install_package("virtualenv")
        self._setup_environment()
        self._install_package("GitPython")
    
    def execute(self, mode: str) -> None:
        """Executes the FNA update or installation process.
        
        Based on the provided mode, either updates or installs FNA, then installs the required FNA libraries.
        
        Args:
            mode (str): The mode of operation, either "update" or "install".
        """
        # Step 1: Install or update FNA
        if mode == "update":
            self._update_fna()
        elif mode == "install":
            self._install_fna()
        else:
            print(f"Invalid mode: {mode} (allowed modes: update, install)")
            sys.exit(1)
            
        # Step 2: Install the FNA libs
        self._install_fna_libs_manager()
        
        # Step 3: ???
        # Step 4: Done!
        print("Done!")
        sys.exit(0)
        
        
    def _install_fna_libs_manager(self) -> None:
        # Check if directory already exists
        self._manage_directory(directory=self._fna_libs_install_path, delete_directory_if_exists=True, create_directory_if_not_exists=False)
        
        if platform.system() == "Darwin":
            self._install_fna_libs_apple()
        else:
            self._install_fna_libs()
        
    def _install_fna_libs(self) -> None:
        """Installs the FNA libraries.
        
        Downloads the FNA libraries archive from the specified remote URL and extracts it to the specified directory.
        """
        import tarfile
        import urllib.request
        
        print("Downloading FNA libs...")
        urllib.request.urlretrieve(self.FNA_LIBS_LINK, os.path.join(self._base_directory, "fnalibs.tar.bz2"))
        
        print("Extracting FNA libs...")
        with tarfile.open(os.path.join(self._base_directory, "fnalibs.tar.bz2")) as tar:
            tar.extractall(self._fna_libs_install_path)
            
        self._remove_file_system_entry(os.path.join(self._base_directory, "fnalibs.tar.bz2"))
    
    def _install_fna_libs_apple(self) -> None:
        print("Todo!")
        
    def _install_fna(self) -> None:
        """Installs the FNA repository.
        
        Clones the FNA repository from the specified remote URL. If the repository already exists locally, it attempts
        to update it instead.
        """
        from git import Repo
        
        print("Cloning FNA repo...")
        try:
            Repo.clone_from(self.FNA_REPO, self._fna_repo_install_path, multi_options=["--recursive"])

            repo = Repo(self._fna_repo_install_path)
            for submodule in repo.submodules:
                submodule.update(init=True, recursive=True)
        except Exception:
            print("Repo already exists, attempting to update...")
            self._update_fna()
            return
        
    def _update_fna(self) -> None:
        """Updates the FNA repository.
        
        Pulls the latest changes from the origin remote of the FNA repository.
        """
        from git import Repo
        print ("Updating FNA repo...")
        
        repo = Repo(self._fna_repo_install_path)
        repo.remotes.origin.pull()

        for submodule in repo.submodules:
            submodule.update(init=True, recursive=True)
        print("All submodules have been updated.")
    
    @staticmethod
    def _is_virtual_environment() -> bool:
        """Checks if the current Python environment is a virtual environment.
        
        Determines if the script is running within a virtual environment by examining sys attributes.
        
        Returns:
            bool: True if the current environment is a virtual environment, False otherwise.
        """
        return hasattr(sys, "real_prefix") or (hasattr(sys, "base_prefix") and sys.base_prefix != sys.prefix)
    
    def _setup_environment(self) -> None:
        """Sets up a Python virtual environment if one is not already active.
        
        Checks if already running within a virtual environment. If not, creates and activates a new virtual environment
        at a predefined path.
        """
        print("Setting up environment...")
        if self._is_virtual_environment():
            print("  Already in virtual environment!")
            return
        
        # setup virtual environment
        setupVirtualEnvironment = not self._manage_directory(directory=self._virtual_environment_path, delete_directory_if_exists=True, create_directory_if_not_exists=False)
            
        print("  Setting up virtual environment...")
        if setupVirtualEnvironment:
            subprocess.call([sys.executable, "-m", "virtualenv", self._virtual_environment_path])
        
        print("  Activating virtual environment...")
        activation_file = os.path.join(self._virtual_environment_path, "Scripts", "activate_this.py")        
        exec(open(activation_file).read(), {'__file__': activation_file})
            
    def _manage_directory(self, *, directory: str, delete_directory_if_exists: bool, create_directory_if_not_exists: bool) -> bool:
        """Manages a directory, ensuring it exists and is a directory, not a file.
        
        Args:
            directory (str): The path to the directory to manage.
            create_directory_if_not_exists (bool): Whether to create the directory if it doesn't exist.
        
        Returns:
            bool: True if the directory existed and was not a file, False otherwise.
        """
        if os.path.exists(directory):
            if not os.path.isfile(directory):
                return True
            if delete_directory_if_exists:
                self._remove_file_system_entry(directory)
            
        if create_directory_if_not_exists:
            os.makedirs(directory)
        return False

    @staticmethod
    def _remove_file_system_entry(path: str) -> None:
        """Removes a file or directory from the file system.
        
        Deletes the specified file or directory.  If the path does not exist, no action is taken.
                
        Args:
            path (str): The path to the file or directory to remove.
        """
        if not os.path.exists(path):
            return

        if os.path.isfile(path):
            os.remove(path)
        else:
            shutil.rmtree(path)
    
    @staticmethod
    def _install_package(package: str) -> None:
        """Installs a Python package if it is not already installed.

        Uses pip to install the specified package. If the package is already installed, no action is taken.
        
        Args:
            package (str): The name of the package to install.
        """
        if util.find_spec(package) is None:
            subprocess.call([sys.executable, "-m", "pip", "install", package])

# Get arguments
mode = "install" # sys.argv[1]  # update or install
install_directory = "C:\\Users\\ricky\\RiderProjects\\ProjectTools\\Templates\\Velentr.INDIVIDUAL_SUPPORT\\.fna" #sys.argv[2]  # the directory for the FNA installation

updator = FnaUpdator(directory=install_directory)
updator.execute(mode=mode)

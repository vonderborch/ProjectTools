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
    
    Based on https://fna-xna.github.io/docs/1%3A-Setting-Up-FNA/#chapter-3-download-and-update-fna
    """

    FNA_REPO: str = "https://github.com/FNA-XNA/FNA"
    """str: The URL for the FNA repository."""
    
    FNA_LIBS_LINK: str = "https://fna.flibitijibibo.com/archive/fnalibs.tar.bz2"
    """str: The URL for the FNA libs archive."""
    
    FNA_LIBS_APPLE_REPO: str = "https://github.com/TheSpydog/fnalibs-apple-builder"
    """str: The URL for the FNA libs Apple repository."""
    
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
    
    def execute(self) -> None:
        """Executes the FNA update or installation process.
        
        Based on the provided mode, either updates or installs FNA, then installs the required FNA libraries.
        
        Args:
            mode (str): The mode of operation, either "update" or "install".
        """
        # Step 1: Install or update the FNA repo
        self._clone_or_update_repo(
            repo=self.FNA_REPO, directory=self._fna_repo_install_path, clone_multi_options=["--recursive"]
        )
            
        # Step 2: Install the FNA libs
        self._install_fna_libs_manager()
        
        # Step 3: ???
        # Step 4: Done!
        print("Done!")
        sys.exit(0)
        
        
    def _install_fna_libs_manager(self) -> None:
        """Manages the installation of FNA libraries for the current platform.
        
        Prepares the installation directory and triggers platform-specific library installation methods.
        
        Returns:
            None
        """
        # Check if directory already exists
        self._manage_directory(
            directory=self._fna_libs_install_path, delete_directory_if_exists=True, create_directory_if_not_exists=False
        )

        self._install_fna_libs()
        # If on MacOS, we can also compile the libs for iOS and MacOS!
        if platform.system() == "Darwin":
            self._install_fna_libs_apple()
        
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
        """Compiles the FNA libraries for Apple platforms.
        
        Based on: https://github.com/TheSpydog/fnalibs-apple-builder
        """
        fna_libs_apple_path = os.path.join(self._base_directory, "fnalibs-apple-builder")
        
        # Step 1: Clone or update the required repository
        self._clone_or_update_repo(
            repo=self.FNA_LIBS_APPLE_REPO,
            directory=fna_libs_apple_path,
            clone_multi_options=[],
        )
        
        # Step 2: Run the update script
        print("Running FNA libs Apple update script...")
        subprocess.call(["/bin/bash", os.path.join(fna_libs_apple_path, "updatelibs.sh")], cwd=fna_libs_apple_path)
        
        # Step 3: Execute the build script
        print("Building FNA libs...")
        subprocess.call(["/bin/bash", os.path.join(fna_libs_apple_path, "buildlibs.sh"), "macos"], cwd=fna_libs_apple_path)
        subprocess.call(["/bin/bash", os.path.join(fna_libs_apple_path, "buildlibs.sh"), "ios"], cwd=fna_libs_apple_path)
        subprocess.call(["/bin/bash", os.path.join(fna_libs_apple_path, "buildlibs.sh"), "ios-sim"], cwd=fna_libs_apple_path)
        
        # Step 4: Copy the built libraries to the FNA libs directory
        print("Copying FNA libs...")
        shutil.copytree(os.path.join(fna_libs_apple_path, "bin"), self._fna_libs_install_path)
        
        # Step 5: Run the cleanup script
        print("Cleaning up...")
        subprocess.call(["/bin/bash", os.path.join(fna_libs_apple_path, "buildlibs.sh"), "clean"], cwd=fna_libs_apple_path)
        
    def _clone_or_update_repo(self, repo: str, directory: str, clone_multi_options: list[str]) -> None:
        """Clones or updates a Git repository with specified options.
        
        Handles repository management by either cloning a new repository or updating an existing one, including submodule initialization.
        
        Args:
            repo: The URL of the Git repository to clone or update.
            directory: The local directory path where the repository will be cloned or updated.
            clone_multi_options: Additional options to use during repository cloning.
        
        Returns:
            None
        
        Raises:
            Exception: If the specified directory is a file instead of a directory.
        """
        from git import Repo
        
        # Step 1: Check if we need to clone or update the repo...
        clone = True
        update = False
        
        repoName = os.path.basename(repo)
        
        if os.path.exists(directory):
            if os.path.isfile(directory):
                raise Exception(f"Directory {directory} already exists and is a file?")
            if os.path.exists(os.path.join(directory, ".git")):
                update = True
                clone = False
        
        # Step 2: Clone the repo?
        if clone:
            try:
                print(f"Cloning {repoName}...")
                Repo.clone_from(repo, directory, multi_options=clone_multi_options)
            except Exception:
                print("  Repo already exists, attempting to update...")
                update = True
        
        # Step 3: Update the repo?
        if update:
            print(f"  Updating {repoName}...")
            repo = Repo(directory)
            repo.remotes.origin.pull()
        
        # Step 4: Update the submodules!
        print("  Updating submodules...")
        repo = Repo(directory)
        for submodule in repo.submodules:
            submodule.update(init=True, recursive=True)
        
        print("  Done!")
    
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
        setupVirtualEnvironment = not self._manage_directory(
            directory=self._virtual_environment_path,
            delete_directory_if_exists=True,
            create_directory_if_not_exists=False,
        )
            
        print("  Setting up virtual environment...")
        if setupVirtualEnvironment:
            subprocess.call([sys.executable, "-m", "virtualenv", self._virtual_environment_path])
        
        print("  Activating virtual environment...")
        if platform.system() == "Windows":
            activation_file = os.path.join(self._virtual_environment_path, "Scripts", "activate_this.py")
        else:
            activation_file = os.path.join(self._virtual_environment_path, "bin", "activate_this.py")
        exec(open(activation_file).read(), {'__file__': activation_file})
            
    def _manage_directory(
        self, *, directory: str, delete_directory_if_exists: bool, create_directory_if_not_exists: bool
    ) -> bool:
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
install_directory = "/Users/christianwebber/Library/CloudStorage/Dropbox/Projects/ProjectTools/Templates/Velentr.INDIVIDUAL_SUPPORT/.fna"  # the directory for the FNA installation
#install_directory = "C:\\Users\\ricky\\RiderProjects\\ProjectTools\\Templates\\Velentr.INDIVIDUAL_SUPPORT\\.fna"  # the directory for the FNA installation
#install_directory = sys.argv[1]  # the directory for the FNA installation

updator = FnaUpdator(directory=install_directory)
updator.execute()

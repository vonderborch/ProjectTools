import os

if os.path.exists(".gitpersonalaccesstoken") and os.path.isfile(".gitpersonalaccesstoken"):
    with open(".gitpersonalaccesstoken", "w") as file:
        file.write("GITPERSONALACCESSTOKEN_HERE")
else:
    raise Exception("WTF?")

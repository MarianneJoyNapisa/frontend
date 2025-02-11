# HomeownersMS Guide to Git Pulling/Pushing

## How to Pull From Repo For the First Time?

1. Create a new folder
2. Open cmd sa kana nga folder
3. `git clone https://github.com/VulpritProoze/ELNET_HomeownersMS`
4. `cd name_sa_folder`
5. `git fetch origin`
6. `git checkout development`
7. `git pull origin development`


## How to Push?

1. Go to the root directory of your folder (I think ELNET_HomeownersMS ang name)
2. `git add .`
3. `git commit -m "message here"`
4. `git push origin development`

## How to Pull Again After Naay Changes sa Remote Repo
Like nausab ang Development branch (e.g. naay lain ni push ug changes)

1. Go to root directory sa current folder
2. `git checkout development`
3. `git pull origin development`

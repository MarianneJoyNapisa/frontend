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
4. `git checkout <branch sa imo ipush>`
5. `git remote add origin https://github.com/VulpritProoze/ELNET_HomeownersMS`
6. `git push`

## How to Pull Again After Naay Changes sa Remote Repo
Like nausab ang Development branch (e.g. naay lain ni push ug changes)

1. Go to root directory sa current folder
2. `git checkout <branch sa imo pullan>`
3. `git remote add origin https://github.com/VulpritProoze/ELNET_HomeownersMS`
4. `git pull`

## How to Push to "frontend"
1. `git add .`
2. `git commit -m "messagehere"`
3. `git checkout frontend`
4. `git remote add origin https://github.com/VulpritProoze/ELNET_HomeownersMS.git`
5. `git push`

### Take Note:
 1. If naa kay usbon sa local repo nimo (sa imo computer), you have to always remember to `git pull origin development`. This pulls changes from remote repo (sa Github). This ensures nga smooth imo work inig `git push`.

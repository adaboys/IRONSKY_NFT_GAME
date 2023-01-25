# Switch to main
git checkout main

printf "Merge develop to main? (y/*): "
read merge_develop
if [[ $merge_develop == "y" ]]; then
	git merge develop
fi

printf "Push to remote main? (y/*) "
read push_remote_main
if [[ $push_remote_main == "y" ]]; then
	git push
fi

# Switch back to develop
git checkout develop
git branch

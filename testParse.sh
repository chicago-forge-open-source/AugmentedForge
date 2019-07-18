result=$(echo 'cat //test-run/@result' | xmllint --shell ./test-results/editMode/results.xml | awk -F'[="]' '!/>/{print $(NF-1)}')
echo $result
if test "$result" = "Passed"
then
	echo "THE PASSING"
	exit 0;
else
	echo "THE FAILING"
	exit 1;
fi
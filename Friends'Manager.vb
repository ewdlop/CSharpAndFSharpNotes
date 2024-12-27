Sub Main
	
End Sub

Public Class Form1

	' A list to store friends' details
	Private friendsList As New List(Of String)


	' Add Friend Button Click

	Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

		Dim name As String = txtName.Text

		Dim birthday As String = txtBirthday.Text


		If String.IsNullOrWhiteSpace(name) OrElse String.IsNullOrWhiteSpace(birthday) Then

			MessageBox.Show("Please enter both name and birthday.", "Error")

			Return
		End If


		' Add friend to the list

		Dim friendInfo As String = $"Name: {name}, Birthday: {birthday}"

		friendsList.Add(friendInfo)

		MessageBox.Show($"{name} added to your friends list!", "Success")


		' Clear input fields

		txtName.Clear()

		txtBirthday.Clear()

	End Sub


	' View Friends Button Click

	Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
		' Clear the list box
		lstFriends.Items.Clear()

		' Display all friends

		For Each friend In friendsList
			lstFriends.Items.Add(friend)
		Next

	End Sub

	Private Sub btnCheckBirthday_Click(sender As Object, e As EventArgs) Handles btnCheckBirthday.Click
		Dim today As String = DateTime.Now.ToString("MM/dd")
		Dim birthdayFriends As New List(Of String)

		For Each friendInfo In friendsList
			Dim parts As String() = friendInfo.Split(","c)
			If parts.Length > 1 Then
				Dim birthday As String = parts(1).Replace("Birthday: ", "").Trim()
				If birthday.StartsWith(today) Then
					birthdayFriends.Add(parts(0).Replace("Name: ", "").Trim())
				End If
			End If
		Next

		If birthdayFriends.Count > 0 Then
			MessageBox.Show($"Today is the birthday of: {String.Join(", ", birthdayFriends)}", "Birthday Reminder")
		Else
			MessageBox.Show("No birthdays today!", "Birthday Reminder")
		End If
	End Sub


End Class

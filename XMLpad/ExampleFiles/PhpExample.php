<?php
// Connect to the database
$servername = "localhost";
$username = "username";
$password = "password";
$dbname = "myDB";

$conn = mysqli_connect($servername, $username, $password, $dbname);
if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
}

// Create the books table
$sql = "CREATE TABLE IF NOT EXISTS books (
    id INT(6) UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    title VARCHAR(30) NOT NULL,
    author VARCHAR(30) NOT NULL,
    year INT(4) NOT NULL
)";

if (mysqli_query($conn, $sql)) {
    echo "Table created successfully";
} else {
    echo "Error creating table: " . mysqli_error($conn);
}

// Insert some books into the table
$sql = "INSERT INTO books (title, author, year) VALUES
    ('The Great Gatsby', 'F. Scott Fitzgerald', 1925),
    ('To Kill a Mockingbird', 'Harper Lee', 1960),
    ('1984', 'George Orwell', 1949)";

if (mysqli_query($conn, $sql)) {
    echo "Records inserted successfully";
} else {
    echo "Error inserting records: " . mysqli_error($conn);
}

// Retrieve all the books from the table
$sql = "SELECT * FROM books";
$result = mysqli_query($conn, $sql);

if (mysqli_num_rows($result) > 0) {
    // Output each book as an HTML table row
    echo "<table><tr><th>Title</th><th>Author</th><th>Year</th></tr>";
    while ($row = mysqli_fetch_assoc($result)) {
        echo "<tr><td>" . $row["title"] . "</td><td>" . $row["author"] . "</td><td>" . $row["year"] . "</td></tr>";
    }
    echo "</table>";
} else {
    echo "No books found";
}

// Close the database connection
mysqli_close($conn);
?>

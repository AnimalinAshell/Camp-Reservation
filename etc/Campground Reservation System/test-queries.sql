SELECT * FROM park

SELECT * FROM campground

DECLARE @input_campground_id AS int = 1
DECLARE @input_to_date AS date = '2018-02-19'
DECLARE @input_from_date AS date = '2018-02-20'
SELECT
	s.site_number,
	s.max_occupancy,
	s.accessible,
	s.max_rv_length,
	s.utilities,
	c.daily_fee
FROM site s
LEFT JOIN reservation r ON r.site_id = s.site_id
JOIN campground c ON c.campground_id = s.campground_id
WHERE
	c.campground_id = @input_campground_id AND
	s.site_id NOT IN (
		SELECT rs.site_id
		FROM reservation rs
		WHERE
			(@input_to_date > from_date AND @input_to_date < to_date) OR
			(@input_from_date > from_date AND @input_from_date < to_date)
	)
GROUP BY
	s.site_number,
	s.max_occupancy,
	s.accessible,
	s.max_rv_length,
	s.utilities,
	c.daily_fee;


SELECT * FROM site

SELECT * FROM reservation
SELECT * FROM park

SELECT * FROM campground

DECLARE @input_campground_id AS int = 1
DECLARE @input_from_date AS date = '2018-04-23'
DECLARE @input_to_date AS date = '2018-04-24'
SELECT
	s.site_number,
	s.max_occupancy,
	s.accessible,
	s.max_rv_length,
	s.utilities,
	c.daily_fee
FROM site s
JOIN campground c ON c.campground_id = s.campground_id
WHERE
	c.campground_id = @input_campground_id AND
	c.open_from_mm <= MONTH(@input_from_date) AND
	c.open_to_mm >= MONTH(@input_to_date) AND
	((c.open_from_mm = 1 AND c.open_to_mm = 12) OR
	(YEAR(@input_from_date) = YEAR(@input_to_date))) AND
	s.site_id NOT IN (
		SELECT r.site_id
		FROM reservation r
		WHERE
			(@input_to_date > from_date AND @input_to_date <= to_date) OR
			(@input_from_date >= from_date AND @input_from_date < to_date) OR
			(@input_from_date <= from_date AND @input_to_date > to_date)
	);


SELECT * FROM site

SELECT * FROM reservation

SELECT * FROM campground
import React, { useState } from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  MenuItem,
  Box,
  Paper,
  Grid,
} from "@mui/material";
import StraightenIcon from "@mui/icons-material/Straighten";

export default function MeasurementPage() {
  const [formData, setFormData] = useState({
    height: "",
    chest: "",
    weight: "",
    pstStatus: "",
  });
  const [result, setResult] = useState(null);

  // Example GR standards (adjust as needed)
  const GR_STANDARDS = {
    height: 165,
    chest: 80,
    weight: 60,
    pstStatus: "Pass",
  };

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const evaluateCandidate = () => {
    const { height, chest, weight, pstStatus } = formData;
    const pass =
      Number(height) >= GR_STANDARDS.height &&
      Number(chest) >= GR_STANDARDS.chest &&
      Number(weight) >= GR_STANDARDS.weight &&
      pstStatus === GR_STANDARDS.pstStatus;

    setResult(pass ? "✅ Candidate Allowed to Proceed" : "❌ Candidate Failed");
  };

  return (
    <Container maxWidth={false}>
      <Paper sx={{ p: 4, mt: 2, minHeight: '80vh' }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', pb: 2, mb: 4 }}>
          <Typography variant="h4" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <StraightenIcon fontSize="large" /> Physical Measurement Evaluation (PST)
          </Typography>
        </Box>

        <Grid container spacing={6}>
          <Grid item xs={12} lg={8}>
            <Typography variant="h6" gutterBottom color="text.secondary">Enter Candidate Measurements</Typography>
            <Grid container spacing={4}>
              <Grid item xs={12} sm={4}>
                <TextField
                  fullWidth
                  label="Height (cm)"
                  name="height"
                  type="number"
                  value={formData.height}
                  onChange={handleChange}
                  required
                  variant="outlined"
                  InputProps={{ style: { fontSize: '1.2rem' } }}
                />
              </Grid>
              <Grid item xs={12} sm={4}>
                <TextField
                  fullWidth
                  label="Chest (cm)"
                  name="chest"
                  type="number"
                  value={formData.chest}
                  onChange={handleChange}
                  required
                  variant="outlined"
                  InputProps={{ style: { fontSize: '1.2rem' } }}
                />
              </Grid>
              <Grid item xs={12} sm={4}>
                <TextField
                  fullWidth
                  label="Weight (kg)"
                  name="weight"
                  type="number"
                  value={formData.weight}
                  onChange={handleChange}
                  required
                  variant="outlined"
                  InputProps={{ style: { fontSize: '1.2rem' } }}
                />
              </Grid>
              <Grid item xs={12} sm={6}>
                <TextField
                  select
                  fullWidth
                  label="Override Status"
                  name="pstStatus"
                  value={formData.pstStatus}
                  onChange={handleChange}
                  required
                  helperText="Use checks for automatic pass/fail unless override needed"
                >
                  <MenuItem value="Pass">Pass</MenuItem>
                  <MenuItem value="Fail">Fail</MenuItem>
                </TextField>
              </Grid>
              <Grid item xs={12} sm={6} display="flex" alignItems="center">
                <Button
                  variant="contained"
                  size="large"
                  fullWidth
                  onClick={evaluateCandidate}
                  sx={{ height: 56 }}
                >
                  Evaluate Measurements
                </Button>
              </Grid>
            </Grid>
          </Grid>

          <Grid item xs={12} lg={4}>
            <Paper variant="outlined" sx={{ p: 3, height: '100%', bgcolor: '#fcfcfc', display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
              <Typography variant="overline" color="text.secondary" align="center">Evaluation Result</Typography>
              {result ? (
                <Box textAlign="center" my={2}>
                  <Typography variant="h4" color={result.includes("✅") ? "success.main" : "error.main"} fontWeight="bold">
                    {result.includes("✅") ? "QUALIFIED" : "DISQUALIFIED"}
                  </Typography>
                  <Typography variant="body1" sx={{ mt: 1 }}>
                    {result.includes("✅") ? "Candidate meets all physical criteria." : "Candidate does not meet required standards."}
                  </Typography>
                </Box>
              ) : (
                <Typography align="center" color="text.disabled" variant="h5">
                  Waiting for Input...
                </Typography>
              )}
            </Paper>
          </Grid>
        </Grid>
      </Paper>
    </Container>
  );
}

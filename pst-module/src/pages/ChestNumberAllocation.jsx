import React, { useState } from "react";
import {
  Box,
  Button,
  Container,
  Grid,
  Paper,
  TextField,
  Typography,
} from "@mui/material";
import ConfirmationNumberIcon from "@mui/icons-material/ConfirmationNumber";

function ChestNumberAllocation() {
  const [applicationNo, setApplicationNo] = useState("");
  const [candidateName, setCandidateName] = useState("");
  const [chestNumber, setChestNumber] = useState(null);
  const [errors, setErrors] = useState({});

  // Application Number format: PB2026XXXX
  const isValidApplicationNo = (value) => {
    const pattern = /^PB2026\d{4}$/;
    return pattern.test(value);
  };

  const validateForm = () => {
    let tempErrors = {};

    if (!applicationNo.trim()) {
      tempErrors.applicationNo = "Application number is required";
    } else if (!isValidApplicationNo(applicationNo)) {
      tempErrors.applicationNo = "Invalid format (Expected: PB2026XXXX)";
    }

    if (!candidateName.trim()) {
      tempErrors.candidateName = "Candidate name is required";
    } else if (!/^[A-Za-z ]+$/.test(candidateName)) {
      tempErrors.candidateName = "Candidate name should contain only letters";
    }

    setErrors(tempErrors);
    return Object.keys(tempErrors).length === 0;
  };

  const allocateChestNumber = () => {
    if (!validateForm()) return;

    const generatedChestNumber =
      "CH-" + Math.floor(1000 + Math.random() * 9000);

    setChestNumber(generatedChestNumber);
  };

  return (
    <Container maxWidth={false} sx={{ height: 'calc(100vh - 100px)', overflow: 'hidden' }}>
      <Paper sx={{ p: 3, mt: 1, height: '100%', display: 'flex', flexDirection: 'column' }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', pb: 1, mb: 2 }}>
          <Typography variant="h5" color="primary" sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
            <ConfirmationNumberIcon fontSize="large" /> Chest Number Allocation
          </Typography>
          <Typography variant="subtitle1" color="text.secondary" sx={{ mt: 1 }}>
            Generate unique chest numbers for physical tests.
          </Typography>
        </Box>

        <Grid container spacing={6}>
          {/* Input Section */}
          <Grid item xs={12} lg={5}>
            <Paper variant="outlined" sx={{ p: 4 }}>
              <Typography variant="h6" gutterBottom>Candidate Details</Typography>
              <Grid container spacing={3} sx={{ mt: 1 }}>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Application Number"
                    placeholder="PB2026XXXX"
                    value={applicationNo}
                    onChange={(e) => setApplicationNo(e.target.value.toUpperCase())}
                    error={!!errors.applicationNo}
                    helperText={errors.applicationNo}
                    inputProps={{ maxLength: 10 }}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Candidate Name"
                    placeholder="Enter Candidate Name"
                    value={candidateName}
                    onChange={(e) => setCandidateName(e.target.value)}
                    error={!!errors.candidateName}
                    helperText={errors.candidateName}
                  />
                </Grid>
                <Grid item xs={12}>
                  <Button
                    fullWidth
                    variant="contained"
                    size="large"
                    onClick={allocateChestNumber}
                    disabled={!!chestNumber}
                    sx={{ mt: 2, height: 50 }}
                  >
                    Allocate Chest Number
                  </Button>
                </Grid>
              </Grid>
            </Paper>
          </Grid>

          {/* Receipt Section */}
          <Grid item xs={12} lg={7}>
            {chestNumber ? (
              <Paper
                elevation={6}
                sx={{
                  p: 6,
                  height: '100%',
                  display: 'flex',
                  flexDirection: 'column',
                  alignItems: 'center',
                  justifyContent: 'center',
                  bgcolor: "#e8f5e9",
                  border: "4px solid #2e7d32",
                  textAlign: "center",
                  position: 'relative'
                }}
              >
                <Typography variant="overline" sx={{ letterSpacing: 3, fontSize: '1rem', color: 'success.dark' }}>
                  Initial Allocation Success
                </Typography>
                <Typography variant="h2" sx={{ fontWeight: 900, color: 'primary.main', my: 4, letterSpacing: 2 }}>
                  {chestNumber}
                </Typography>

                <Box sx={{ width: '100%', maxWidth: 400, borderTop: '1px solid black', pt: 2, display: 'flex', justifyContent: 'space-between' }}>
                  <Box>
                    <Typography variant="caption" display="block" color="text.secondary">Name</Typography>
                    <Typography variant="h6">{candidateName}</Typography>
                  </Box>
                  <Box textAlign="right">
                    <Typography variant="caption" display="block" color="text.secondary">App No</Typography>
                    <Typography variant="h6">{applicationNo}</Typography>
                  </Box>
                </Box>
              </Paper>
            ) : (
              <Box sx={{
                height: '100%',
                minHeight: 300,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                justifyContent: 'center',
                bgcolor: 'background.paper',
                borderRadius: 2,
                border: '3px dashed #e0e0e0',
                color: 'text.secondary'
              }}>
                <ConfirmationNumberIcon sx={{ fontSize: 80, opacity: 0.2, mb: 2 }} />
                <Typography variant="h6" color="text.secondary">Ready to Allocate</Typography>
                <Typography variant="body2">Enter details to generate chest number</Typography>
              </Box>
            )}
          </Grid>
        </Grid>
      </Paper>
    </Container>
  );
}

export default ChestNumberAllocation;

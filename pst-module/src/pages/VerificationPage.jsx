import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../styles/verification.css';
import {
  Dialog, DialogTitle, DialogContent, DialogActions, Button,
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Paper, IconButton, Box, Typography, CircularProgress, Alert, TextField, Container
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import CheckCircleIcon from '@mui/icons-material/CheckCircle';
import CancelIcon from '@mui/icons-material/Cancel';
import SearchIcon from '@mui/icons-material/Search';
import { endpoints } from '../config';

const VerificationPage = () => {
  const navigate = useNavigate();

  // Search State
  const [searchAppId, setSearchAppId] = useState('');

  // Modal State
  const [openModal, setOpenModal] = useState(false);
  const [selectedCandidate, setSelectedCandidate] = useState(null);
  const [verificationState, setVerificationState] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [fetchError, setFetchError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);

  // Handlers
  const handleSearchAndOpen = async () => {
    if (!searchAppId.trim()) {
      alert("Please enter an Application Number");
      return;
    }

    setOpenModal(true);
    setSelectedCandidate(null); // Clear previous
    setIsLoading(true);
    setFetchError(null);
    setVerificationState({});

    try {
      const response = await fetch(endpoints.candidate(searchAppId.trim()));
      if (!response.ok) {
        if (response.status === 404) {
          throw new Error("Candidate not found using this Application Number.");
        }
        throw new Error(`Server returned ${response.status}: ${response.statusText}`);
      }
      const details = await response.json();

      // Inject the ID we searched for if not returned explicitly (though it should be)
      if (!details.ApplicationNo) details.ApplicationNo = searchAppId;

      setSelectedCandidate(details);

      // Initialize verification state
      const initialVerifyState = {};
      const keys = [
        "Maharashtra_Domicile", "MaharashtraDomicileCertNo",
        "FarmerSuicideReportNo", "KarnatakaDomicileCertNo",
        "NCCCertificateNo", "CasteCertificateNo"
      ];

      keys.forEach(key => {
        initialVerifyState[key] = null; // null = pending
      });
      setVerificationState(initialVerifyState);

    } catch (err) {
      console.error("Failed to fetch candidate details:", err);
      setFetchError(err.message || "Failed to load data. Use correct Application No.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleCloseModal = () => {
    setOpenModal(false);
    setSelectedCandidate(null);
  };

  const verifyField = (field, status) => {
    setVerificationState(prev => ({
      ...prev,
      [field]: status
    }));
  };

  const submitVerification = async () => {
    if (!selectedCandidate) return;

    // Validation: Ensure ALL fields have been clicked (value is not null)
    const values = Object.values(verificationState);
    const pendingFields = values.some(val => val === null);

    if (pendingFields) {
      alert("Please verify ALL fields (Correct or False) before submitting.");
      return;
    }

    const hasFailure = values.includes('false');
    const finalStatus = hasFailure ? "FAIL" : "PASS";

    setIsSubmitting(true);
    try {
      const response = await fetch(endpoints.verify, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          applicationNo: selectedCandidate.ApplicationNo || searchAppId,
          status: finalStatus
        })
      });

      if (!response.ok) throw new Error("Failed to submit verification");

      alert(`Verification saved: ${finalStatus}`);
      handleCloseModal();
      setSearchAppId(''); // Optional: clear search after success
    } catch (err) {
      alert(`Error: ${err.message}`);
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <Container maxWidth="md" sx={{ mt: 8, textAlign: 'center' }}>

      <Paper elevation={3} sx={{ p: 6, borderRadius: 2 }}>
        <Typography variant="h4" gutterBottom color="primary" fontWeight="bold">
          Document Verification
        </Typography>
        <Typography variant="subtitle1" color="text.secondary" sx={{ mb: 4 }}>
          Enter the Candidate Application Number to view and verify documents.
        </Typography>

        <Box sx={{ display: 'flex', gap: 2, alignItems: 'center', justifyContent: 'center' }}>
          <TextField
            label="Application Number"
            variant="outlined"
            placeholder="e.g. APP-2026-001"
            value={searchAppId}
            onChange={(e) => setSearchAppId(e.target.value.toUpperCase())}
            sx={{ width: '300px' }}
            onKeyPress={(e) => e.key === 'Enter' && handleSearchAndOpen()}
          />
          <Button
            variant="contained"
            size="large"
            sx={{ height: '56px', px: 4 }}
            onClick={handleSearchAndOpen}
            startIcon={<SearchIcon />}
          >
            View Full Form
          </Button>
        </Box>
      </Paper>

      {/* Verification Modal */}
      <Dialog
        open={openModal}
        onClose={handleCloseModal}
        maxWidth="md"
        fullWidth
      >
        <DialogTitle sx={{ m: 0, p: 2, bgcolor: '#f5f5f5', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h6">Application Verification</Typography>
          <IconButton onClick={handleCloseModal}>
            <CloseIcon />
          </IconButton>
        </DialogTitle>
        <DialogContent dividers>
          {isLoading ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 5 }}>
              <CircularProgress />
            </Box>
          ) : fetchError ? (
            <Alert severity="error" sx={{ mt: 2 }}>{fetchError}</Alert>
          ) : selectedCandidate ? (
            <TableContainer component={Paper} elevation={0} variant="outlined">
              <Table>
                <TableHead>
                  <TableRow sx={{ bgcolor: '#e3f2fd' }}>
                    <TableCell width="40%"><strong>Field Name</strong></TableCell>
                    <TableCell width="30%"><strong>Value</strong></TableCell>
                    <TableCell width="30%" align="center"><strong>Verification</strong></TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {[
                    { label: "Application No", key: "ApplicationNo" },
                    { label: "Maharashtra Domicile", key: "Maharashtra_Domicile" },
                    { label: "MH Domicile Cert No", key: "MaharashtraDomicileCertNo" },
                    { label: "Farmer Suicide Report No", key: "FarmerSuicideReportNo" },
                    { label: "Karnataka Domicile Cert", key: "KarnatakaDomicileCertNo" },
                    { label: "NCC Certificate No", key: "NCCCertificateNo" },
                    { label: "Caste Certificate No", key: "CasteCertificateNo" }
                  ].map((field) => (
                    <TableRow key={field.key}>
                      <TableCell>{field.label}</TableCell>
                      <TableCell sx={{ fontFamily: 'monospace', fontSize: '1.1em' }}>
                        {selectedCandidate[field.key] || "N/A"}
                      </TableCell>
                      <TableCell align="center">
                        <Box sx={{ display: 'flex', gap: 1, justifyContent: 'center' }}>
                          <Button
                            variant={verificationState[field.key] === 'correct' ? 'contained' : 'outlined'}
                            color="success"
                            size="small"
                            onClick={() => verifyField(field.key, 'correct')}
                            startIcon={<CheckCircleIcon />}
                          >
                            Correct
                          </Button>
                          <Button
                            variant={verificationState[field.key] === 'false' ? 'contained' : 'outlined'}
                            color="error"
                            size="small"
                            onClick={() => verifyField(field.key, 'false')}
                            startIcon={<CancelIcon />}
                          >
                            False
                          </Button>
                        </Box>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          ) : null}
        </DialogContent>
        <DialogActions sx={{ p: 2, bgcolor: '#f5f5f5' }}>
          <Button onClick={handleCloseModal} variant="outlined" color="inherit">
            Close
          </Button>
          <Button
            onClick={submitVerification}
            variant="contained"
            color="primary"
            disabled={isSubmitting || isLoading || !!fetchError}
          >
            {isSubmitting ? 'Saving...' : 'Submit Verification'}
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default VerificationPage;